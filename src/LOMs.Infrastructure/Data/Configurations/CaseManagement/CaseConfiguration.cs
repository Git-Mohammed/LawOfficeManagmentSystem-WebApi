using LOMs.Domain.Cases;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasKey(c => c.Id)
               .IsClustered(false);
        builder.Property(c => c.Id)
               .ValueGeneratedNever();

        builder.Property(c => c.CaseNumber)
               .HasMaxLength(150);

        builder.Property(c => c.CaseSubject)
               .HasMaxLength(1000)
               .IsUnicode(true);

        builder.Property(c => c.ClientRequests)
               .HasMaxLength(2000)
               .IsUnicode(true);

        builder.Property(c => c.EstimatedReviewDate)
               .IsRequired();

        builder.Property(c => c.Status)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsUnicode(false);
        builder.HasIndex(c => c.Status)
               .HasDatabaseName("IX_Cases_Status");

        builder.Property(c => c.PartyRole)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsUnicode(false)
               .HasComment("Client's role: Plaintiff or Defendant");
        builder.HasIndex(c => c.PartyRole)
               .HasDatabaseName("IX_Cases_PartyRole");

        builder.Property(c => c.AssignedEmployeeId)
               .IsRequired();
        builder.HasIndex(c => c.AssignedEmployeeId)
               .HasDatabaseName("IX_Cases_AssignedEmployeeId");
        builder.HasOne(c => c.Employee)
               .WithMany(e => e.Cases)
               .HasForeignKey(c => c.AssignedEmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Cases", SchemaNames.GetName(DatabaseSchema.Case));
    }
}
