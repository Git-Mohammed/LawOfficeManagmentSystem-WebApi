using LOMs.Contract.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases;

public class CreateCaseRequest
{
    [StringLength(50, ErrorMessage = "Case number must not exceed 50 characters.")]
    public string? CaseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Court type is required.")]
    public Guid CourtTypeId { get; set; }

    [StringLength(250, ErrorMessage = "Case subject must not exceed 250 characters.")]
    public string? CaseSubject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Party role is required.")]
    [EnumDataType(typeof(PartyRole), ErrorMessage = "Party role is invalid.")]
    public PartyRole PartyRole { get; set; }

    [StringLength(1000, ErrorMessage = "Client request details must not exceed 1000 characters.")]
    public string? ClientRequestDetails { get; set; } = string.Empty;

    [Required(ErrorMessage = "Estimated review date is required.")]
    public DateOnly EstimatedReviewDate { get; set; }

    public bool IsDraft { get; set; }

    [StringLength(1000, ErrorMessage = "Lawyer opinion must not exceed 1000 characters.")]
    public string? LawyerOpinion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Assigned employee is required.")]
    public Guid AssignedEmployeeId { get; set; }

    [MinLength(1, ErrorMessage = "At least one client must be added.")]
    [Required(ErrorMessage = "Clients list is required.")]
    public List<CaseClientRequest> Clients { get; set; } = new();
}
