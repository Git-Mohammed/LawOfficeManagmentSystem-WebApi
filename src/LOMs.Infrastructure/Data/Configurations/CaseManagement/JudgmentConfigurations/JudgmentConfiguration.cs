using LOMs.Domain.Cases.Judgments;
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
            .HasMaxLength(50);

        builder.Property(j => j.Type).HasConversion<string>()
            .IsRequired();

        builder.Property(j => j.SubType).HasConversion<string>()
            .IsRequired();

        builder.Property(j => j.IssuedOn)
            .IsRequired();

        // Relationships
        builder.HasOne(j => j.Case)
            .WithMany(c => c.Judgments)
            .HasForeignKey(j => j.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Table name
        builder.ToTable("Judgments", "CaseManagement");
    }
}