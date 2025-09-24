using LiteBus.Queries.Abstractions;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Common.Models;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LOMs.Application.Features.Contracts.Queries.GetContractList.Paged;

public sealed class GetPagedContractListQueryHandler(
    IAppDbContext context,
    ILogger<GetPagedContractListQueryHandler> logger)
    : IQueryHandler<GetPagedContractListQuery, Result<PaginatedList<ContractListItemDto>>>
{
    public async Task<Result<PaginatedList<ContractListItemDto>>> HandleAsync(
        GetPagedContractListQuery query,
        CancellationToken ct)
    {
        if (query.PageIndex < 1)
        {
            logger.LogWarning("Invalid page number: {PageNumber}", query.PageIndex);
            return Error.Validation("Pagination.InvalidPageNumber", "Page number must be at least 1.");
        }

        if (query.PageSize < 1)
        {
            logger.LogWarning("Invalid page size: {PageSize}", query.PageSize);
            return Error.Validation("Pagination.InvalidPageSize", "Page size must be between 1 and 100.");
        }

        var totalCount = await context.Contracts.CountAsync(ct);

        var contracts = await context.Contracts
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAtUtc)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
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


        var paginated = new PaginatedList<ContractListItemDto>(
            items: contracts,
            pageIndex: query.PageIndex,
            pageSize: query.PageSize,
            totalItemsCount: totalCount
        );

        return paginated;
    }
}
