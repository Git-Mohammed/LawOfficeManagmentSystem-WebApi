namespace LOMs.Domain.Identity.DomainEvents;

public record UserLoggedOutEvent(string UserId, string Username, DateTime Timestamp);
