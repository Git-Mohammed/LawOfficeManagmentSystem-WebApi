using LiteBus.Commands.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.ClientFiles;
using LOMs.Application.Features.Contracts.Services;
using LOMs.Application.Utilities;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Common.Enums;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.POAs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LOMs.Application.Features.Cases.Commands.CreateCase;

public sealed class CreateCaseCommandHandler(
    IMapper mapper,
    ILogger<CreateCaseCommandHandler> logger,
    IAppDbContext context,
    IImageService imageService,
    IClientFileFactory clientFileFactory,
    IContractFactory contractFactory
) : ICommandHandler<CreateCaseCommand, Result<CaseDto>>
{
    private readonly ILogger<CreateCaseCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IClientFileFactory _clientFileFactory = clientFileFactory;
    private readonly IContractFactory _contractFactory = contractFactory;

    public async Task<Result<CaseDto>> HandleAsync(CreateCaseCommand message, CancellationToken cancellationToken = default)
    {

        // All existing validation and entity creation logic for clients and cases...
        if (message.Clients is null || !message.Clients.Any())
        {
            _logger.LogWarning("No clients provided for case creation.");
            return CaseErrors.InvalidClients;
        }

        if (!string.IsNullOrWhiteSpace(message.CaseNumber))
        {
            var caseNumberExists = await _context.Cases.AnyAsync(x => x.CaseNumber == message.CaseNumber, cancellationToken);
            if (caseNumberExists)
            {
                _logger.LogWarning("Duplicate case number detected: {CaseNumber}", message.CaseNumber);
                return CaseErrors.DuplicateCaseNumber(message.CaseNumber);
            }
        }

        if (message.AssignedOfEmployeeId == Guid.Empty)
        {
            return CaseErrors.EmptyEmployeeId;
        }

        var employeeExists = await _context.Employees
            .AnyAsync(x => x.Id == message.AssignedOfEmployeeId, cancellationToken);

        if (!employeeExists)
        {
            return CaseErrors.AssignedEmployeeNotFound(message.AssignedOfEmployeeId);
        }


        var courtType = await _context.CourtTypes.FirstOrDefaultAsync(x => x.Id == message.CourtTypeId);
        if(courtType is null)
        {
            _logger.LogWarning("No clients provided for case creation.");

            return CaseErrors.InvalidCourtType;

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

                return CaseErrors.ClientNotFoundWithIds(missingClientIds);
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
                .ToHashSetAsync(cancellationToken);

            if (duplicates.Any())
            {
                _logger.LogWarning("Duplicate national IDs detected: {NationalIds}", string.Join(", ", duplicates));
                return CaseErrors.DuplicateNationalIds(duplicates);
            }
        }

        _logger.LogInformation("Starting case creation for CaseNumber: {CaseNumber}", message.CaseNumber);

        var validationResult = await ValidateCaseAndClientsAsync(message, existingClients, courtType,cancellationToken);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var (caseEntity, clients, clientFiles, clientCases) = validationResult.Value;
        var savedFilePaths = new List<string>();

        if (message.HasContracts && !message.Contracts.Any())
            return CaseErrors.ContractFilesMissing;

        var contractsResult = await ProcessContractsAsync(message.Contracts, caseEntity, courtType, savedFilePaths, cancellationToken);
        if (contractsResult.IsError)
        {
            return contractsResult.Errors;
        }

        if (message.HasPOAs && !message.Contracts.Any())
            return CaseErrors.PoasMissing;

        var poasResult = await ProcessPoasAsync(message.POAs, caseEntity, savedFilePaths, cancellationToken);
        if (poasResult.IsError)
        {
            return poasResult.Errors;
        }

        // Final persistence in a single try-catch block
        try
        {
            _logger.LogInformation("Persisting case and related entities to database...");

            await _context.Cases.AddAsync(caseEntity, cancellationToken);

            if (clients.Count > 0)
                await _context.Clients.AddRangeAsync(clients, cancellationToken);

            if (clientFiles.Count > 0)
                await _context.ClientFiles.AddRangeAsync(clientFiles, cancellationToken);

            if (clientCases.Count > 0)
                await _context.ClientCases.AddRangeAsync(clientCases, cancellationToken);

            if (message.Contracts.Count > 0 && message.HasContracts)
                await _context.Contracts.AddRangeAsync(contractsResult.Value, cancellationToken);

            if (message.POAs.Count > 0 && message.HasPOAs)
                await _context.POAs.AddRangeAsync(poasResult.Value, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Case creation completed successfully. CaseId: {CaseId}", caseEntity.Id);
            return _mapper.Map<Case, CaseDto>(caseEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating case. Rolling back transaction.");
            foreach (var path in savedFilePaths)
            {
                _imageService.DeleteImage(path);
            }
            return CaseErrors.UnexpectedFailure;
        }
    }

    

    // Private helper methods for a cleaner main handler

    private async Task<Result<(Case caseEntity, List<Client> clients, List<ClientFile> clientFiles, List<ClientCase> clientCases)>> ValidateCaseAndClientsAsync(CreateCaseCommand message, IReadOnlyDictionary<Guid, Client> existingClients,CourtType courtType, CancellationToken cancellationToken)
    {
        // ... (This method would contain all the validation and client-related logic from your original code) ...

        var clients = new List<Client>();
        var clientFiles = new List<ClientFile>();
        var clientCases = new List<ClientCase>();

        // Case creation
        var caseResult = Case.Create(
            Guid.NewGuid(),
            courtType.Id,
            message.CaseNumber,
            message.CaseSubject,
            message.PartyRole,
            message.ClientRequests,
            message.EstimatedReviewDate,
            message.IsDraft ? CaseStatus.Draft : CaseStatus.Pending,
            message.LawyerOpinion,
            message.AssignedOfEmployeeId
        );
        if (caseResult.IsError)
        {
            _logger.LogWarning("Case creation failed due to validation errors.");
            return caseResult.Errors;
        }
        var caseEntity = caseResult.Value;

        // Clients processing
        // ... (all client validation and handling logic, like the loops in your original code) ...
        _logger.LogInformation("Case entity created with ID: {CaseId}", caseEntity.Id); 
        foreach (var clientDto in message.Clients)
        {
            if (clientDto is ExistingCaseClientModel existing)
            {
                var existingResult = await HandleExistingClient(existing, caseEntity.Id, courtType, existingClients, cancellationToken);
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
                var newResult = await HandleNewClient(newClient, caseEntity.Id, courtType, cancellationToken);
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
                return CaseErrors.UnknownClientType;
            }
        }

        return  (caseEntity, clients, clientFiles, clientCases);
    }

    private async Task<Result<List<Contract>>> ProcessContractsAsync(IEnumerable<CreateContractWithCaseCommand> contractCommands, Case caseEntity,CourtType courtType, List<string> savedFilePaths, CancellationToken cancellationToken)
    {
        var contracts = new List<Contract>();
        if (contractCommands is null || !contractCommands.Any())
        {
            return contracts; // Return an empty list if no contracts are provided
        }

        try
        {
            foreach (var command in contractCommands)
            {
                var filePath = await _imageService.SaveImageAsync(command.AttachmentFileStream, command.AttachmentFileName, ImageFolder.Contracts);
                savedFilePaths.Add(filePath);

                var contractResult = await _contractFactory.CreateAsync(caseEntity.Id, courtType, command.ContractType, command.IssueDate, command.ExpiryDate, command.BaseAmount, command.InitialPayment, filePath, command.IsAssigned);

                if (contractResult.IsError)
                {
                    // Clean up already saved files
                    foreach (var path in savedFilePaths)
                    {
                        _imageService.DeleteImage(path);
                    }
                    return contractResult.Errors;
                }
                contracts.Add(contractResult.Value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving contract files. Rolling back.");
            return CaseErrors.SaveContractFileFailure;
        }

        return contracts;
    }

    private async Task<Result<List<POA>>> ProcessPoasAsync(IEnumerable<CreatePOAWithCaseCommand> poaCommands, Case caseEntity, List<string> savedFilePaths, CancellationToken cancellationToken)
    {
        var poas = new List<POA>();
        if (poaCommands is null || !poaCommands.Any())
        {
            return poas; // Return an empty list if no POAs are provided
        }

        try
        {
            foreach (var command in poaCommands)
            {
                var filePath = await _imageService.SaveImageAsync(command.AttachmentFileStream, command.AttachmentFileName, ImageFolder.POAs);
                savedFilePaths.Add(filePath);

                var poaResult = POA.Create(Guid.NewGuid(), caseEntity.Id, command.POANumber, command.IssueDate, command.IssuingAuthority, filePath);

                if (poaResult.IsError)
                {
                    // Clean up already saved files
                    foreach (var path in savedFilePaths)
                    {
                        _imageService.DeleteImage(path);
                    }
                    return poaResult.Errors;
                }
                poas.Add(poaResult.Value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving POA files. Rolling back.");
            return CaseErrors.ImageSaveFailure;
        }

        return poas;
    }
    
    private async Task<Result<(ClientFile? clientFile, ClientCase clientCase)>> HandleExistingClient(
        ExistingCaseClientModel request,
        Guid caseId,
        CourtType courtType,
        IReadOnlyDictionary<Guid, Client> existingClients,
        CancellationToken ct)
    {
        if (!existingClients.TryGetValue(request.ExistingClientId, out var client))
        {
            _logger.LogWarning("Existing client not found: {ClientId}", request.ExistingClientId);
            return CaseErrors.ClientNotFoundWithId(request.ExistingClientId);
        }
        var currentHijriYear = HijriDateConverter.GetCurrentHijriYear();

        var existingFile = client.ClientFiles.FirstOrDefault(x => x.CourtTypeCode == courtType.Code && x.HijriYear == currentHijriYear);

        ClientFile? clientFile = null;

        if (existingFile is null)
        {
            var clientFileResult = await _clientFileFactory.CreateAsync(client.Id, courtType, ct);
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

    private async Task<Result<(Client client, ClientFile clientFile, ClientCase clientCase)>> HandleNewClient(
        NewCaseClientModel request,
        Guid caseId,
        CourtType courtType,
        CancellationToken ct)
    {
        var personResult = Person.Create(
            Guid.NewGuid(),
            request.Client.Person.FullName,
            request.Client.Person.NationalId,
            request.Client.Person.CountryCode,
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

        var clientFileResult = await _clientFileFactory.CreateAsync(clientResult.Value.Id, courtType, ct);
        if (clientFileResult.IsError)
        {
            _logger.LogWarning("Failed to create client file for new client: {ClientId}", clientResult.Value.Id);
            return clientFileResult.Errors;
        }

        var clientCase = new ClientCase(caseId, clientResult.Value.Id, clientFileResult.Value.Id);
        return (clientResult.Value, clientFileResult.Value, clientCase);
    }
}
