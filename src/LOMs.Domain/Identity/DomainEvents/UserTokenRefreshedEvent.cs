namespace LOMs.Domain.Identity.DomainEvents;

public record UserTokenRefreshedEvent(string UserId, string Username, DateTime Timestamp);
