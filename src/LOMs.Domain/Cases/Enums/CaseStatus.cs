namespace LOMs.Domain.Cases.Enums
{
    public enum CaseStatus
    {
        Draft = 0,       // Case is being prepared, not yet submitted
        Pending = 1,     // Awaiting review or action
        Processing = 2,  // Actively being handled
        Complete = 3,    // Successfully resolved
        Cancelled = 4    // Terminated before resolution
    }
}
