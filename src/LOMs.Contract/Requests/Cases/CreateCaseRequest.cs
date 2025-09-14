using LOMs.Contract.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases;

public class CreateCaseRequest
{
    [StringLength(50, ErrorMessage = "رقم القضية يجب ألا يتجاوز 50 حرف.")]
    public string? CaseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "نوع المحكمة مطلوب.")]
    public Guid CourtTypeId { get; set; }

    [StringLength(250, ErrorMessage = "موضوع القضية يجب ألا يتجاوز 250 حرف.")]
    public string? CaseSubject { get; set; } = string.Empty;

    [Required(ErrorMessage = "دور الطرف مطلوب.")]
    [EnumDataType(typeof(PartyRole), ErrorMessage = "دور الطرف غير صالح.")]
    public PartyRole PartyRole { get; set; }

    [StringLength(1000, ErrorMessage = "تفاصيل طلب العميل يجب ألا تتجاوز 1000 حرف.")]
    public string? ClientRequestDetails { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاريخ المراجعة التقديري مطلوب.")]
    public DateOnly EstimatedReviewDate { get; set; }

    public bool IsDraft { get; set; }

    [StringLength(1000, ErrorMessage = "رأي المحامي يجب ألا يتجاوز 1000 حرف.")]
    public string? LawyerOpinion { get; set; } = string.Empty;

    [Required(ErrorMessage = "الموظف المسؤول مطلوب.")]
    public Guid AssignedEmployeeId { get; set; }

    [MinLength(1, ErrorMessage = "يجب إضافة عميل واحد على الأقل.")]
    [Required(ErrorMessage = "قائمة العملاء مطلوبة.")]
    public List<CaseClientRequest> Clients { get; set; } = new();
}
