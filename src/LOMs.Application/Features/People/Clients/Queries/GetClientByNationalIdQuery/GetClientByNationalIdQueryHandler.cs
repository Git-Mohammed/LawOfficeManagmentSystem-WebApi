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

public sealed class GetClientByNationalIdQueryHandler(
    ILogger<GetCaseByIdQueryHandler> logger,
    IAppDbContext context,
    IMapper mapper)
    : IQueryHandler<GetClientByNationalIdQuery, Result<ClientDto>>
{
    private readonly ILogger<GetCaseByIdQueryHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ClientDto>> HandleAsync(GetClientByNationalIdQuery request, CancellationToken cancellationToken)
    {
        var clientEntity = await _context.Clients.AsNoTracking()
           .Include(x => x.Person)
           .FirstOrDefaultAsync(x => x.Person.NationalId == request.NationalId);
        if (clientEntity is null)
        {
            _logger.LogWarning("Client with NationalId {NationalId} not found.", request.NationalId);
            return ClientErrors.Client_NotFoundWithNationalId(request.NationalId);
        }

        _logger.LogInformation("Client with NationalId {NationalId} not found.", request.NationalId);

        return _mapper.Map<Client, ClientDto>(clientEntity);
    }
}
