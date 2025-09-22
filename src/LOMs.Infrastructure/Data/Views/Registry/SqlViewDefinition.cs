using LOMs.Infrastructure.Enums;

namespace LOMs.Infrastructure.Data.Views.Registry;

public record SqlViewDefinition(DatabaseSchema Schema, string ViewName, string Sql);