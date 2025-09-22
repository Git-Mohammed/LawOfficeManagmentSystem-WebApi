namespace LOMs.Infrastructure.Enums
{
    public enum DatabaseSchema
    {
        Identity,     // Handles user identity, roles, and permissions
        User,         // Handles people, clients, employees
        Case,         // Manages legal case lifecycle and metadata
        Finance,      // Tracks billing, payments, and financial records
        File          // Stores and indexes uploaded documents and attachments
    }
}
