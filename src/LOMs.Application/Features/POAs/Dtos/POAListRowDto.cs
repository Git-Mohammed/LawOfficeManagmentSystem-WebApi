namespace LOMs.Application.Features.POAs.Dtos;

/// <summary>
/// Represents a single POA entry in a list view.
/// </summary>
public sealed class POAListRowDto
{
    public string POANumber { get; init; } = default!;
    public DateOnly IssueDate { get; init; }
    public string IssuingAuthority { get; init; } = default!;
    public Guid EmployeeIdCreatedBy { get; init; }
    public string EmployeeNameCreatedBy { get; init; } = default!;
    public DateTimeOffset CreatedDate { get; init; } = default!;
    public string FilePath { get; init; } = default!;
}
