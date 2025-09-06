using System.Security.Claims;

namespace LOMs.Application.Features.Auth.Dtos;
    public sealed record AppUserDto(string UserId, string Email,string Username, IList<string> Roles, IList<Claim> Claims);

    //public class AppUserDto
    //{
    //    public string Id { get; init; } = null!;
    //    public string Email { get; init; } = null!;
    //    public string[] Roles { get; init; } = null!;
    //}

