namespace LOMs.Infrastructure.Enums;

internal enum DatabaseSchema
{
    Identity,     // Handles user identity, roles, and permissions
    User,         // Handles poplpe, client, employes
    Case,         // Manages legal case lifecycle and metadata
    Finance,      // Tracks billing, payments, and financial records
    File          // Stores and indexes uploaded documents and attachments
}
