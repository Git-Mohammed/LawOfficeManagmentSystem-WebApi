using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases;

public class CreatePOAWithCaseRequest 
{
    [Required(ErrorMessage = "رقم الوكالة مطلوب.")]
    [MaxLength(50, ErrorMessage = "رقم الوكالة يجب ألا يتجاوز 50 حرفًا.")]
    public string POANumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاريخ إصدار الوكالة مطلوب.")]
    public DateOnly IssueDate { get; set; }

    [Required(ErrorMessage = "جهة إصدار الوكالة مطلوبة.")]
    [MaxLength(200, ErrorMessage = "جهة الإصدار يجب ألا تتجاوز 200 حرف.")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [Required(ErrorMessage = "مرفق الوكالة مطلوب.")]
    public IFormFile AttachmentFile { get; set; } = null!;
}