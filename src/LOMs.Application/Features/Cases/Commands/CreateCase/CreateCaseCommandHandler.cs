using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Enums.CourtTypes;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

public sealed class CreateCaseCommandHandler(
    IMapper mapper,
    ILogger<CreateCaseCommandHandler> logger,
    IAppDbContext context
) : ICommandHandler<CreateCaseCommand, Result<CaseDto>>
{
    private readonly ILogger<CreateCaseCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CaseDto>> HandleAsync(CreateCaseCommand message, CancellationToken cancellationToken = default)
    {
        if (message.Clients is null || !message.Clients.Any())
            return Error.Validation("CreateCase.InvalidClients", "At least one client must be provided.");

        // ✅ Preload all existing clients in ONE query (avoid N+1 problem)
        var existingClientIds = message.Clients
            .OfType<ExistingCaseClientModel>()
            .Select(x => x.ExistingClientId)
            .ToList();

        var clients = new List<Client>();
        var clientFiles = new List<ClientFile>();
        var clientCases = new List<ClientCase>();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var caseResult = Case.Create(
                Guid.NewGuid(),
                message.CaseNumber,
                message.CourtType,
                message.CaseNotes,
                message.Role,
                message.ClientRequests,
                message.EstimatedReviewDate,
                message.IsDraft ? CaseStatus.Draft : CaseStatus.Pending,
                message.LawyerOpinion,
                message.AssignedOfficer
            );

            if (caseResult.IsError)
                return caseResult.Errors;

            var @case = caseResult.Value;



            var existingClients = await _context.Clients
                .Include(c => c.Person)
                .Include(c => c.ClientFiles)
                .Where(c => existingClientIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, cancellationToken);


            foreach (var clientDto in message.Clients)
            {
                if (clientDto is ExistingCaseClientModel existing)
                {
                    var existingResult = HandleExistingClient(existing, @case.Id, message.CourtType, existingClients);
                    if (existingResult.IsError)
                        return existingResult.Errors;

                    var (clientFile, clientCase) = existingResult.Value;

                    if (clientFile is not null) // only new file
                        clientFiles.Add(clientFile);

                    clientCases.Add(clientCase);
                }
                else if (clientDto is NewCaseClientModel newClient)
                {
                    var newResult = HandleNewClient(newClient, @case.Id, message.CourtType);
                    if (newResult.IsError)
                        return newResult.Errors;

                    var (client, clientFile, clientCase) = newResult.Value;

                    clients.Add(client);       // ✅ only new clients saved
                    clientFiles.Add(clientFile);
                    clientCases.Add(clientCase);
                }
                else
                {
                    return Error.Unexpected("CreateCase.UnknownClientType", "Unknown client type.");
                }
            }
            await _context.Cases.AddAsync(@case, cancellationToken);
            // ✅ Persist only new clients + new files + cases
            if (clients.Count > 0)
                await _context.Clients.AddRangeAsync(clients, cancellationToken);

            if (clientFiles.Count > 0)
                await _context.ClientFiles.AddRangeAsync(clientFiles, cancellationToken);

            if (clientCases.Count > 0)
             await _context.ClientCases.AddRangeAsync(clientCases, cancellationToken);

           await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);


            return _mapper.Map<Case, CaseDto>(@case); // ✅ mapper shortcut
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating case.");
            await transaction.RollbackAsync(cancellationToken);
            return Error.Unexpected("CreateCase.Failure", "An unexpected error occurred while creating the case.");
        }
    }

    private Result<(ClientFile? clientFile, ClientCase clientCase)> HandleExistingClient(
        ExistingCaseClientModel request,
        Guid caseId,
        CourtType courtType,
        IReadOnlyDictionary<Guid, Client> existingClients)
    {

        if (!existingClients.TryGetValue(request.ExistingClientId, out var client))
            return Error.NotFound("CreateCase.ClientNotFound", $"Client with ID {request.ExistingClientId} not found.");

        // ✅ Reuse existing file if possible
        var existingFile = client.ClientFiles.FirstOrDefault(x => x.CourtType == courtType);

        ClientFile? clientFile = null;

        if (existingFile is null)
        {
            var clientFileResult = ClientFile.Create(Guid.NewGuid(), client.Id, courtType);
            if (clientFileResult.IsError)
                return clientFileResult.Errors;

            clientFile = clientFileResult.Value;
        }

        var clientCase = new ClientCase(caseId, client.Id, (clientFile ?? existingFile)!.Id);
        return (clientFile, clientCase);
    }

    private Result<(Client client, ClientFile clientFile, ClientCase clientCase)> HandleNewClient(
        NewCaseClientModel request,
        Guid caseId,
        CourtType courtType)
    {
        var exists = _context.People.Any(p => p.NationalId == request.Client.Person.NationalId);
        if (exists)
            return Error.Validation("CreateCase.DuplicateNationalId",
                $"A client with National ID {request.Client.Person.NationalId} already exists.");

        var personResult = Person.Create(
            Guid.NewGuid(),
            request.Client.Person.FullName,
            request.Client.Person.NationalId,
            request.Client.Person.BirthDate,
            request.Client.Person.PhoneNumber,
            request.Client.Person.Address
        );

        if (personResult.IsError)
            return personResult.Errors;

        var clientResult = Client.Create(Guid.NewGuid(), personResult.Value);
        if (clientResult.IsError)
            return clientResult.Errors;

        var clientFileResult = ClientFile.Create(Guid.NewGuid(), clientResult.Value.Id, courtType);
        if (clientFileResult.IsError)
            return clientFileResult.Errors;

        var clientCase = new ClientCase(caseId, clientResult.Value.Id, clientFileResult.Value.Id);
        return (clientResult.Value, clientFileResult.Value, clientCase);
    }
}
