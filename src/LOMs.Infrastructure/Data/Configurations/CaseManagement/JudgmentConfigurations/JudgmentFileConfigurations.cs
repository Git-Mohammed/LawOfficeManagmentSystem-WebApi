using LOMs.Domain.Cases.Judgments;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement.JudgmentConfigurations;

public class JudgmentFileConfiguration : IEntityTypeConfiguration<JudgmentFile>
{
    public void Configure(EntityTypeBuilder<JudgmentFile> builder)
    {
        builder.HasKey(jf => jf.Id);
        builder.Property(jf => jf.Id)
               .ValueGeneratedNever();

        builder.Property(jf => jf.JudgmentId)
               .IsRequired();

        builder.Property(jf => jf.Filename)
               .IsRequired()
               .HasMaxLength(200)
               .IsUnicode(false);

        builder.Property(jf => jf.Filepath)
               .IsRequired()
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(jf => jf.Extension)
               .HasMaxLength(8)
               .IsUnicode(false);

        builder.Property(jf => jf.ContentType)
               .HasMaxLength(100)
               .IsUnicode(false);

        builder.Property(jf => jf.FileType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(30)
               .IsUnicode(false);

        builder.Property(jf => jf.Size)
               .HasColumnType("bigint")
               .IsRequired();

        //  Relationship → Judgment
        builder.HasOne(jf => jf.Judgment)
               .WithMany(j => j.Files)
               .HasForeignKey(jf => jf.JudgmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(jf => jf.JudgmentId);

        builder.HasIndex(jf => new { jf.JudgmentId, jf.FileType });

        builder.ToTable("JudgmentFiles", SchemaNames.GetName(DatabaseSchema.Case));
    }
}
