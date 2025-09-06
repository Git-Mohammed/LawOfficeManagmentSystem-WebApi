using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Cases.Dtos;
using LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Domain.Cases;
using LOMs.Domain.Common.Results;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.People.Clients.Queries.GetClientByNationalIdQuery;

public sealed class GetClientByIdQueryHandler(
    ILogger<GetCaseByIdQueryHandler> logger,
    IAppDbContext context,
    IMapper mapper)
    : IQueryHandler<GetClientByIdQuery, Result<ClientDto>>
{
    private readonly ILogger<GetCaseByIdQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ClientDto>> HandleAsync(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var clientEntity = await _context.Clients.AsNoTracking()
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Id == request.ClientId);
        if (clientEntity is null)
        {
            _logger.LogWarning("Client with Id {ClientId} not found.", request.ClientId);
            return ClientErrors.Client_NotFoundWithId(request.ClientId);
        }

        _logger.LogInformation("Client with Id {ClientId} retrieved successfully.", request.ClientId);

        return _mapper.Map<Client, ClientDto>(clientEntity);
    }
}
