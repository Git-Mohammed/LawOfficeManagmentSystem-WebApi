using System;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.CourtSesstions;

public sealed class UpdateCourtSessionRequest
{
    [Required(ErrorMessage = "Scheduled date is required.")]
    [DataType(DataType.DateTime)]
    [FutureDate(ErrorMessage = "Scheduled date must be in the future.")]
    public DateTimeOffset NewDate { get; init; }

    [Required(ErrorMessage = "Assigned tasks are required.")]
    [MinLength(5, ErrorMessage = "Assigned tasks must be at least 5 characters.")]
    public string AssignedTasks { get; init; } = string.Empty;

    [Required(ErrorMessage = "Assigned employee ID is required.")]
    public Guid AssignedEmployeeId { get; init; }
}
public sealed class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTimeOffset date)
            return false;

        return date > DateTimeOffset.UtcNow;
    }
}
