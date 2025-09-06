using LiteBus.Commands.Abstractions;
using LOMs.Domain.Common.Results;

public sealed record LogoutCommand(string RefreshToken) : ICommand<Result<Success>>;