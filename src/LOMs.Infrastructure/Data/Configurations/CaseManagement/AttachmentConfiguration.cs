using LOMs.Domain.Cases.Attachments;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        // 1) Primary Key (GUID from domain, never database‐generated)
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
               .ValueGeneratedNever();

        // 2) Case FK & Index
        builder.Property(a => a.CaseId)
               .IsRequired();
        builder.HasIndex(a => a.CaseId);

        // 3) AttachmentName: up to 200 chars, ASCII only
        builder.Property(a => a.AttachmentName)
               .IsRequired()
               .HasMaxLength(200)
               .IsUnicode(false);

        // 4) Filename: up to 200 chars, ASCII only
        builder.Property(a => a.Filename)
               .IsRequired()
               .HasMaxLength(200)
               .IsUnicode(false);

        // 5) Filepath: up to 500 chars, ASCII only
        builder.Property(a => a.Filepath)
               .IsRequired()
               .HasMaxLength(500)
               .IsUnicode(false);

        // 6) Extension: up to 8 chars (e.g. ".docx"), ASCII only
        builder.Property(a => a.Extension)
               .HasMaxLength(8)
               .IsUnicode(false);

        // 7) ContentType: up to 100 chars (e.g. "application/pdf"), ASCII only
        builder.Property(a => a.ContentType)
               .HasMaxLength(100)
               .IsUnicode(false);

        // 8) FileType: enum→string, up to 30 chars, ASCII only
        builder.Property(a => a.FileType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(30)
               .IsUnicode(false);

        // 9) Size in bytes: use bigint rather than double for exactness
        builder.Property(a => a.Size)
               .IsRequired()
               .HasColumnType("bigint");

        // 10) Relationship
        builder.HasOne(a => a.Case)
               .WithMany(c => c.Attachments)
               .HasForeignKey(a => a.CaseId)
               .OnDelete(DeleteBehavior.Cascade);

        // 11) Table & schema
        builder.ToTable("Attachments", SchemaNames.GetName(DatabaseSchema.Case));
    }
}
