namespace LOMs.Application.Features.Judgments.DTOs;

public class JudgmentDTo
{
    public Guid Id { get; set; }
    public string Number  { get; set; } 
    public string  Type  { get; set; } 
    public string SubType   { get; set; }
    public DateOnly IssuedOn  { get; set; }
    
}