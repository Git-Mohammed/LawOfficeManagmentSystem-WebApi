namespace LOMs.Domain.Identity.DomainEvents;

public record UserLoggedInEvent(string UserId, string Username, DateTime Timestamp);
