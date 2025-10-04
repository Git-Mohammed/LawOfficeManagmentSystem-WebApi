using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LOMs.Contract.Requests.Auth;

public sealed class ChangePasswordRequest
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(11, ErrorMessage = "Username must not exceed 10 characters.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Current password is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "New password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be between 8 and 100 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [UniqueCharacters(5, ErrorMessage = "New password must contain at least 5 unique characters.")]
    public string NewPassword { get; set; } = null!;
}

/// <summary>
/// Custom validation attribute to enforce unique character count.
/// </summary>
public sealed class UniqueCharactersAttribute : ValidationAttribute
{
    private readonly int _minUniqueChars;

    public UniqueCharactersAttribute(int minUniqueChars)
    {
        _minUniqueChars = minUniqueChars;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str)
        {
            int uniqueCount = str.Distinct().Count();
            if (uniqueCount < _minUniqueChars)
            {
                return new ValidationResult(ErrorMessage ?? $"Password must contain at least {_minUniqueChars} unique characters.");
            }
        }

        return ValidationResult.Success;
    }
}
