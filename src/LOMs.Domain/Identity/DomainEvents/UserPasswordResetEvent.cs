namespace LOMs.Domain.Identity.DomainEvents;

public record UserPasswordResetEvent(string UserId, string Username, DateTime Timestamp);
