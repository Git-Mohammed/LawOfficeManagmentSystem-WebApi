namespace LOMs.Infrastructure.Configuration;

public sealed class LOMsSettings
{
    public CourtSessionReminderSettings CourtSessionReminder { get; set; } = new();
    public PaginationSettings Pagination { get; set; } = new();
    public NotificationSettings Notification { get; set; } = new();
    public SecuritySettings Security { get; set; } = new();
}

public sealed class PaginationSettings
{
    public int DefaultPageNumber { get; set; } = 1;
    public int DefaultPageSize { get; set; } = 20;
    public int MaxPageSize { get; set; } = 100;
}


public sealed class NotificationSettings
{
    public bool EnableArabicFallback { get; set; } = true;
    public string DefaultSenderEmail { get; set; } = "noreply@loms.local";
}

public sealed class SecuritySettings
{
    public int MinPasswordLength { get; set; } = 8;
    public bool EnableAuditLogging { get; set; } = true;
    public string[] AllowedIpRanges { get; set; } = Array.Empty<string>();
}

