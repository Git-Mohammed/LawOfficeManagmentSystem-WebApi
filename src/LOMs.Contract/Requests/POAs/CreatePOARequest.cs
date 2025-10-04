using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.POAs;

public class CreatePOARequest : IValidatableObject
{
    [Required(ErrorMessage = "POA number is required.")]
    public string POANumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Issue date is required.")]
    public DateOnly? IssueDate { get; set; }

    [Required(ErrorMessage = "Issuing authority is required.")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [Required(ErrorMessage = "POA attachment file is required.")]
    public IFormFile AttachmentFile { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IssueDate is not null && IssueDate > DateOnly.FromDateTime(DateTime.Today))
        {
            yield return new ValidationResult(
                "Issue date cannot be in the future.",
                new[] { nameof(IssueDate) });
        }

        if (string.IsNullOrWhiteSpace(POANumber))
        {
            yield return new ValidationResult(
                "POA number cannot be empty or whitespace.",
                new[] { nameof(POANumber) });
        }

        if (string.IsNullOrWhiteSpace(IssuingAuthority))
        {
            yield return new ValidationResult(
                "Issuing authority cannot be empty or whitespace.",
                new[] { nameof(IssuingAuthority) });
        }
    }
}
