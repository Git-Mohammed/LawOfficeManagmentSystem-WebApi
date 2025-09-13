namespace LOMs.Application.Features.CourtTypes.Dtos;

public class CourtTypeWithYearsDto
{
  public Guid CourtTypeId { get; set; }
  public string CourtTypeName { get; set; } = null!;
  public List<short> HijriYears { get; set; } = new();
}
