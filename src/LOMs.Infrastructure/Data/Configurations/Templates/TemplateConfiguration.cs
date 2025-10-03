using LOMs.Domain.Templates;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.Templates;

public class TemplateConfiguration : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.HasKey(t => t.Id)
               .IsClustered(false);
        builder.Property(t => t.Id)
               .ValueGeneratedNever();

        builder.Property(t => t.TemplateName)
               .IsRequired()
               .HasMaxLength(200)
               .IsUnicode(false);
        builder.HasIndex(t => t.TemplateName)
               .HasDatabaseName("IX_Templates_TemplateName");

        builder.Property(t => t.Filename)
               .IsRequired()
               .HasMaxLength(200)
               .IsUnicode(false);

        builder.Property(t => t.Filepath)
               .IsRequired()
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(t => t.Extension)
               .HasMaxLength(8)
               .IsUnicode(false);

        builder.Property(t => t.ContentType)
               .HasMaxLength(100)
               .IsUnicode(false);

        builder.Property(t => t.FileType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(30)
               .IsUnicode(false);

        builder.ToTable("Templates", SchemaNames.GetName(DatabaseSchema.Templates));
    }
}
