using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.POAs;

public class CreatePOARequest : IValidatableObject
{
    [Required(ErrorMessage = "معرّف القضية مطلوب.")]
    public Guid CaseId { get; set; }

    [Required(ErrorMessage = "رقم الوكالة مطلوب.")]
    [MaxLength(200, ErrorMessage = "رقم الوكالة يجب ألا يتجاوز 200 حرفًا.")]
    public string POANumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاريخ إصدار الوكالة مطلوب.")]
    public DateOnly? IssueDate { get; set; }

    [Required(ErrorMessage = "جهة إصدار الوكالة مطلوبة.")]
    [MaxLength(200, ErrorMessage = "جهة الإصدار يجب ألا تتجاوز 200 حرف.")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [Required(ErrorMessage = "مرفق الوكالة مطلوب.")]
    public IFormFile AttachmentFile { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IssueDate is not null && IssueDate > DateOnly.FromDateTime(DateTime.Today))
        {
            yield return new ValidationResult("تاريخ إصدار الوكالة لا يمكن أن يكون في المستقبل.", new[] { nameof(IssueDate) });
        }

        if (string.IsNullOrWhiteSpace(POANumber))
        {
            yield return new ValidationResult("رقم الوكالة لا يمكن أن يكون فارغًا أو يحتوي فقط على مسافات.", new[] { nameof(POANumber) });
        }

        if (string.IsNullOrWhiteSpace(IssuingAuthority))
        {
            yield return new ValidationResult("جهة إصدار الوكالة لا يمكن أن تكون فارغة أو تحتوي فقط على مسافات.", new[] { nameof(IssuingAuthority) });
        }
    }
}
