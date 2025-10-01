using LOMs.Contract.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases;

public class UpdateCaseStatusRequest
{
    [Required(ErrorMessage = "New status is required.")]
    [EnumDataType(typeof(CaseStatus), ErrorMessage = "Status value is invalid.")]
    public CaseStatus Status { get; set; }
}
