using LOMs.Domain.Identity;
using Microsoft.AspNetCore.Identity;
namespace LOMs.Infrastructure.Identity;

public class ApplicationUser : IdentityUser 
{
    public bool IsUsingTempPassword { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

