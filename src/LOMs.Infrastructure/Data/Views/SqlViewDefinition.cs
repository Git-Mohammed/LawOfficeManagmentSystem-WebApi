using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Data.Views;

public record SqlViewDefinition(DatabaseSchema Schema, string ViewName, string Sql);