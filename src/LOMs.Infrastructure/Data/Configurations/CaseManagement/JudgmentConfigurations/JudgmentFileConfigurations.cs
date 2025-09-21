using LOMs.Domain.Cases.Judgments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement.JudgmentConfigurations;

public class JudgmentFileConfigurations : IEntityTypeConfiguration<JudgmentFile>
{
    public void Configure(EntityTypeBuilder<JudgmentFile> builder)
    {
        builder.HasKey(jf => jf.Id);
        
        builder.Property(jf => jf.JudgmentId)
            .IsRequired();

        builder.Property(jf => jf.Filename)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(jf => jf.Filepath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(jf => jf.Extension)
            .HasMaxLength(10);

        builder.Property(jf => jf.ContentType)
            .HasMaxLength(100);

        builder.Property(jf => jf.FileType).HasConversion<string>()
            .IsRequired();

        builder.HasOne(x => x.Judgment)
            .WithMany(c => c.Files)
            .HasForeignKey(jf => jf.JudgmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(jf => jf.JudgmentId);
        builder.ToTable("JudgmentFiles", "CaseManagement");
    }
}