namespace LOMs.Infrastructure.Configuration;

public sealed class LOMsSettings
{
    public CourtSessionReminderSettings CourtSessionReminder { get; init; } = new();
    public PaginationSettings Pagination { get; init; } = new();
    public NotificationSettings Notification { get; init; } = new();
    public SecuritySettings Security { get; init; } = new();
}

public sealed class PaginationSettings
{
    public int DefaultPageNumber { get; init; } = 1;
    public int DefaultPageSize { get; init; } = 20;
    public int MaxPageSize { get; init; } = 100;
}


public sealed class NotificationSettings
{
    public int CourtSessionReminderDaysAhead { get; init; } = 1;
    public bool EnableArabicFallback { get; init; } = true;
    public string DefaultSenderEmail { get; init; } = "noreply@loms.local";
}

public sealed class SecuritySettings
{
    public int MinPasswordLength { get; init; } = 8;
    public bool EnableAuditLogging { get; init; } = true;
    public string[] AllowedIpRanges { get; init; } = Array.Empty<string>();
}

