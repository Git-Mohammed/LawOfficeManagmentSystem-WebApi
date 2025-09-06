using Microsoft.AspNetCore.Identity;
namespace LOMs.Infrastructure.Identity;

public class ApplicationUser : IdentityUser 
{
    public string? RefreshToken { get; set; } = null!; // string for only session and array of string for multiple sessions
}

