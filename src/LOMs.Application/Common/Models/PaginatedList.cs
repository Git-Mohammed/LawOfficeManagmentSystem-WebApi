using LOMs.Domain.Common.Results;

namespace LOMs.Application.Common.Models;

public class PaginatedList<T>(
    IReadOnlyCollection<T>? items,
    int pageIndex,
    int pageSize,
    int totalItemsCount)
{
    /// <summary>
    /// Gets the current page index (1-based).
    /// </summary>
    public int PageIndex { get; init; } = pageIndex;

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; init; } = pageSize;

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int TotalItemsCount { get; init; } = totalItemsCount;

    /// <summary>
    /// Gets the items for the current page.
    /// </summary>
    public IReadOnlyCollection<T>? Items { get; init; } = items;

    /// <summary>
    /// Gets the number of items in the current page.
    /// </summary>
    public int ItemsCount => Items?.Count ?? 0;

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages =>
        TotalItemsCount > 0 && PageSize > 0
            ? (int)Math.Ceiling((double)TotalItemsCount / PageSize)
            : 0;

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;
}
