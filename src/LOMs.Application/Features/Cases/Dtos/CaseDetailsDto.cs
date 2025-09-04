using LOMs.Application.Features.People.Clients.Dtos;
using LOMs.Application.Features.People.Employees.DTOs;

namespace LOMs.Application.Features.Cases.Dtos
{
    public class CaseDetailsDto
    {
        public Guid Id { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public string CourtType { get; set; } = string.Empty;
        public string CaseSubject { get; set; } = string.Empty;
        public string PartyRole { get; set; } = string.Empty;
        public string ClientRequestDetails { get; set; } = string.Empty;
        public DateOnly? EstimatedReviewDate { get; set; }
        public string CaseStatus { get; set; }
        public bool HasContracts { get; set; }
        public List<ContractDto> Contracts { get; set; } = new();

        public bool HasPOAs { get; set; }
        public List<POADto> POAs { get; set; } = new();

        public string? LawyerOpinion { get; set; }
        public Guid AssignedEmployeeId { get; set; } 
        public EmployeeDto Employee { get; set; }
        public List<ClientDto> Clients { get; set; } = new();
    }

}
