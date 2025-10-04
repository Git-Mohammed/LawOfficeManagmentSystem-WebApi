using LOMs.Domain.Cases.Judgments;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement.JudgmentConfigurations;

public class JudgmentConfiguration : IEntityTypeConfiguration<Judgment>
{
    public void Configure(EntityTypeBuilder<Judgment> builder)
    {
        // Primary key
        builder.HasKey(j => j.Id);

        // Properties

        builder.Property(j => j.Number)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(j => j.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(j => j.SubType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsUnicode(false);

        builder.Property(j => j.IssuedOn)
            .IsRequired()
            .HasColumnType("date");


        // Relationships
        builder.HasOne(j => j.Case)
            .WithMany(c => c.Judgments)
            .HasForeignKey(j => j.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Table name
        builder.ToTable("Judgments", SchemaNames.GetName(DatabaseSchema.Case));
    }
}
