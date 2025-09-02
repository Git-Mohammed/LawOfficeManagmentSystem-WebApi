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
        _logger.LogInformation("Starting case creation for CaseNumber: {CaseNumber}", message.CaseNumber);

        if (message.Clients is null || !message.Clients.Any())
        {
            _logger.LogWarning("No clients provided for case creation.");
            return CaseErrors.Invalid_Clients;
        }

        if (!string.IsNullOrWhiteSpace(message.CaseNumber))
        {
            var caseNumberExists = await _context.Cases.AnyAsync(x => x.Number == message.CaseNumber, cancellationToken);
            if (caseNumberExists)
            {
                _logger.LogWarning("Duplicate case number detected: {CaseNumber}", message.CaseNumber);
                return CaseErrors.Duplicate_CaseNumber(message.CaseNumber);
            }
        }

        var existingClientIds = message.Clients
            .OfType<ExistingCaseClientModel>()
            .Select(x => x.ExistingClientId)
            .ToList();
        IReadOnlyDictionary<Guid, Client> existingClients = new Dictionary<Guid, Client>();

        if (existingClientIds.Any())
        {
            existingClients = await _context.Clients
                .AsNoTracking()
                .Include(c => c.Person)
                .Include(c => c.ClientFiles)
                .Where(c => existingClientIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, cancellationToken);

            if (existingClients.Count != existingClientIds.Count)
            {
                var missingClientIds = existingClientIds
                    .Where(id => !existingClients.ContainsKey(id))
                    .ToList();

                _logger.LogWarning("Missing clients detected. Expected: {Expected}, Found: {Found}, Missing: {Missing}",
                    existingClientIds.Count, existingClients.Count, string.Join(", ", missingClientIds));

                return CaseErrors.Client_NotFoundWithIds(missingClientIds);
            }
        }


        var existingNationalIds = message.Clients
            .OfType<NewCaseClientModel>()
            .Select(x => x.Client.Person.NationalId)
            .ToList();

        if (existingNationalIds.Any())
        {
            var duplicates = await _context.People
                .Where(p => existingNationalIds.Contains(p.NationalId))
                .Select(p => p.NationalId)
                .ToListAsync(cancellationToken);

            if (duplicates.Any())
            {
                _logger.LogWarning("Duplicate national IDs detected: {NationalIds}", string.Join(", ", duplicates));
                return CaseErrors.Duplicate_NationalIds(duplicates);
            }
        }

        var clients = new List<Client>();
        var clientFiles = new List<ClientFile>();
        var clientCases = new List<ClientCase>();

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
        {
            _logger.LogWarning("Case creation failed due to validation errors.");
            return caseResult.Errors;
        }

        var @case = caseResult.Value;
        _logger.LogInformation("Case entity created with ID: {CaseId}", @case.Id);

        foreach (var clientDto in message.Clients)
        {
            if (clientDto is ExistingCaseClientModel existing)
            {
                var existingResult = HandleExistingClient(existing, @case.Id, message.CourtType, existingClients);
                if (existingResult.IsError)
                {
                    _logger.LogWarning("Failed to process existing client with ID: {ClientId}", existing.ExistingClientId);
                    return existingResult.Errors;
                }

                var (clientFile, clientCase) = existingResult.Value;

                if (clientFile is not null)
                    clientFiles.Add(clientFile);

                clientCases.Add(clientCase);
            }
            else if (clientDto is NewCaseClientModel newClient)
            {
                var newResult = HandleNewClient(newClient, @case.Id, message.CourtType);
                if (newResult.IsError)
                {
                    _logger.LogWarning("Failed to process new client with NationalId: {NationalId}", newClient.Client.Person.NationalId);
                    return newResult.Errors;
                }

                var (client, clientFile, clientCase) = newResult.Value;

                clients.Add(client);
                clientFiles.Add(clientFile);
                clientCases.Add(clientCase);
            }
            else
            {
                _logger.LogError("Unknown client type encountered during case creation.");
                return CaseErrors.Unknown_ClientType;
            }
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("Persisting case and related entities to database...");

            await _context.Cases.AddAsync(@case, cancellationToken);

            if (clients.Count > 0)
                await _context.Clients.AddRangeAsync(clients, cancellationToken);

            if (clientFiles.Count > 0)
                await _context.ClientFiles.AddRangeAsync(clientFiles, cancellationToken);

            if (clientCases.Count > 0)
                await _context.ClientCases.AddRangeAsync(clientCases, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Case creation completed successfully. CaseId: {CaseId}", @case.Id);
            return _mapper.Map<Case, CaseDto>(@case);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating case. Rolling back transaction.");
            await transaction.RollbackAsync(cancellationToken);
            return CaseErrors.Unexpected_Failure;
        }
    }

    private Result<(ClientFile? clientFile, ClientCase clientCase)> HandleExistingClient(
        ExistingCaseClientModel request,
        Guid caseId,
        CourtType courtType,
        IReadOnlyDictionary<Guid, Client> existingClients)
    {
        if (!existingClients.TryGetValue(request.ExistingClientId, out var client))
        {
            _logger.LogWarning("Existing client not found: {ClientId}", request.ExistingClientId);
            return CaseErrors.Client_NotFoundWithId(request.ExistingClientId);
        }

        var existingFile = client.ClientFiles.FirstOrDefault(x => x.CourtType == courtType);

        ClientFile? clientFile = null;

        if (existingFile is null)
        {
            var clientFileResult = ClientFile.Create(Guid.NewGuid(), client.Id, courtType);
            if (clientFileResult.IsError)
            {
                _logger.LogWarning("Failed to create client file for existing client: {ClientId}", client.Id);
                return clientFileResult.Errors;
            }

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
        var personResult = Person.Create(
            Guid.NewGuid(),
            request.Client.Person.FullName,
            request.Client.Person.NationalId,
            request.Client.Person.BirthDate,
            request.Client.Person.PhoneNumber,
            request.Client.Person.Address
        );

        if (personResult.IsError)
        {
            _logger.LogWarning("Failed to create person for new client: {NationalId}", request.Client.Person.NationalId);
            return personResult.Errors;
        }

        var clientResult = Client.Create(Guid.NewGuid(), personResult.Value);
        if (clientResult.IsError)
        {
            _logger.LogWarning("Failed to create client for person: {NationalId}", request.Client.Person.NationalId);
            return clientResult.Errors;
        }

        var clientFileResult = ClientFile.Create(Guid.NewGuid(), clientResult.Value.Id, courtType);
        if (clientFileResult.IsError)
        {
            _logger.LogWarning("Failed to create client file for new client: {ClientId}", clientResult.Value.Id);
            return clientFileResult.Errors;
        }

        var clientCase = new ClientCase(caseId, clientResult.Value.Id, clientFileResult.Value.Id);
        return (clientResult.Value, clientFileResult.Value, clientCase);
    }
}
