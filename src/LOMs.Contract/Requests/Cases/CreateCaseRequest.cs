using LOMs.Contract.Requests.Clients;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases;

/// <summary>
/// Represents the data required to create a new legal case, including metadata and client associations.
/// </summary>
public class CreateCaseRequest
{
    [Required(ErrorMessage = "Case number is required.")]
    public string CaseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Court type is required.")]
    public int CourtType { get; set; }

    public string CaseNotes { get; set; } = string.Empty;

    [Required(ErrorMessage = "Party role is required.")]
    public int Role { get; set; }

    public string ClientRequests { get; set; } = string.Empty;

    public DateOnly? EstimatedReviewDate { get; set; }

    public bool IsDraft { get; set; }

    public string? LawyerOpinion { get; set; }

    [Required(ErrorMessage = "Assigned officer is required.")]
    public string AssignedOfficer { get; set; } = string.Empty;

    /// <summary>
    /// List of clients to associate with the case. Each item may represent an existing client or a new one.
    /// </summary>
    public List<CaseClientRequest> Clients { get; set; } = new();
}
