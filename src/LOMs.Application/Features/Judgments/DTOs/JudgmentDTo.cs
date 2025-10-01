namespace LOMs.Application.Features.Judgments.Dtos
{
    public class JudgmentDto
    {
        public Guid Id { get; set; }
        public string Number  { get; set; } 
        public string  Type  { get; set; } 
        public string SubType   { get; set; }
        public DateOnly IssuedOn  { get; set; }
    
    }
}
