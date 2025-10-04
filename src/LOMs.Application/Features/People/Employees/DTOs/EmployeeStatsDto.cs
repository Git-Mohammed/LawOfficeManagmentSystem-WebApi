namespace LOMs.Application.Features.People.Employees.Dtos;

public sealed class EmployeeStatsDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int InactiveEmployees { get; set; }
}