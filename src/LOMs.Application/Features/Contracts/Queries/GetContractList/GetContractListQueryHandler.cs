using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList;

public sealed class GetContractListQueryHandler(
    IAppDbContext context,
    ILogger<GetContractListQueryHandler> logger)
    : IQueryHandler<GetContractListQuery, Result<List<ContractListItemDto>>>
{
    public async Task<Result<List<ContractListItemDto>>> HandleAsync(
        GetContractListQuery query,
        CancellationToken ct)
    {
        var contracts = await context.Contracts
            .AsNoTracking()
            .Select(c => new
            {
                Contract = c,
                EmployeeName = context.Employees
                    .Where(e => e.UserId == c.CreatedBy)
                    .Select(e => e.Person.FullName)
                    .FirstOrDefault()
            })
            .Select(x => new ContractListItemDto
            {
                ContractId = x.Contract.Id,
                ContractNumber = x.Contract.ContractNumber,
                ContractType = x.Contract.Type,
                IssueDate = x.Contract.IssuedOn,
                ExpiryDate = x.Contract.ExpiresOn,
                TotalAmount = x.Contract.TotalAmount,
                InitialPayment = x.Contract.InitialPayment,
                IsAssigned = x.Contract.IsAssigned,
                EmployeeCreatedBy = x.EmployeeName ?? string.Empty,
                CreatedDate = x.Contract.CreatedAtUtc,
            })
            .ToListAsync(ct);

        return contracts;
    }
}
