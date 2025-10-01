using LOMs.Domain.Cases.Judgments;

public sealed class JudgmentDetailsDto
{
    public Guid JudgmentId { get; set; }
    public Guid CaseId { get; set; }
    public JudgmentType Type { get; set; }       
    public JudgmentSubType SubType { get; set; }  
    public string Number { get; set; } = string.Empty;       
    public DateOnly IssuedOn { get; set; }   
    public Guid EmployeeIdCreatedBy { get; set; } = Guid.Empty;

    public string EmployeeNameCreatedBy { get; set; } = string.Empty; 
    public DateTimeOffset CreatedDate { get; set; }
    public string FilePath { get; set; } = string.Empty;
}
