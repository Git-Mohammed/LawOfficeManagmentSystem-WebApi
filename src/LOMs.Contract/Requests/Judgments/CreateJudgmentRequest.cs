using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LOMs.Contract.Requests.Judgments;

public class CreateJudgmentRequest
{
    [Required(ErrorMessage = "Judgment number is required.")]
    [StringLength(50, ErrorMessage = "Judgment number cannot exceed 50 characters.")]
    public string Number { get; set; } = null!;

    [Required(ErrorMessage = "Judgment type is required.")]
    [StringLength(30, ErrorMessage = "Judgment type cannot exceed 30 characters.")]
    public string Type { get; set; } = null!;

    [Required(ErrorMessage = "Judgment subtype is required.")]
    [StringLength(30, ErrorMessage = "Judgment subtype cannot exceed 30 characters.")]
    public string SubType { get; set; } = null!;

    [Required(ErrorMessage = "Issue date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format for IssuedOn.")]
    public DateOnly IssuedOn { get; set; }

    [Required(ErrorMessage = "Expiry date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format for ExpiresOn.")]
    public DateOnly ExpiresOn { get; set; }
    
    [Required(ErrorMessage = "File is required.")]
    public IFormFile File { get; set; } = null!;
}