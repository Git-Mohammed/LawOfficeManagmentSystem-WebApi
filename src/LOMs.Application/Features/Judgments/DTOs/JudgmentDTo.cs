namespace LOMs.Application.Features.Judgments.Dtos
{
    public class JudgmentDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty; 
        public string Type { get; set; } = string.Empty; 
        public string SubType { get; set; } = string.Empty;
        public DateOnly IssuedOn { get; set; }

    }
}
