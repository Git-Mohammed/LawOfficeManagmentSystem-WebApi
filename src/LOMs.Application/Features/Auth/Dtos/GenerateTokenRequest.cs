using System.Security.Claims;

namespace LOMs.Application.Features.Auth.Dtos;

public sealed record GenerateTokenRequest(string UserId, string Email, string Username,bool IsUsingTempPassowrd, IList<string> Roles);
