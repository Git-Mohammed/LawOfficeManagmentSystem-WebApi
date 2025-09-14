using Microsoft.AspNetCore.Identity;
namespace LOMs.Infrastructure.Identity;

public class ApplicationUser : IdentityUser 
{
    public bool IsUsingTempPassword { get; set; } 
}

