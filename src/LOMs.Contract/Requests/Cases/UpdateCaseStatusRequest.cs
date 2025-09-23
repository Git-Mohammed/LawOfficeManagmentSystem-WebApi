using LOMs.Contract.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases;

public class UpdateCaseStatusRequest
{
    [Required(ErrorMessage = "الحالة الجديدة مطلوبة.")]
    [EnumDataType(typeof(CaseStatus), ErrorMessage = "قيمة الحالة غير صالحة.")]
    public CaseStatus Status { get; set; }
}
