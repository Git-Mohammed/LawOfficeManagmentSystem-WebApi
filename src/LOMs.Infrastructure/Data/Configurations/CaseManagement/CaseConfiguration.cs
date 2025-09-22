using LOMs.Domain.Cases;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasKey(c => c.Id).IsClustered(false);

        builder.Property(c => c.CaseNumber)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(c => c.CaseSubject)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(c => c.ClientRequests)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(c => c.AssignedEmployeeId)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The ID of the employee assigned to follow up on the case");

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>() // Stored as integer
            .HasComment("Current status of the case: 0 = Draft, 1 = Pending, 2 = In Progress, 3 = Completed, 4 = Cancelled");

        builder.Property(c => c.PartyRole)
            .IsRequired()
            .HasConversion<int>() // Stored as integer
            .HasComment("Client's role in the case: 1 = Plaintiff, 2 = Defendant");

        builder.HasOne(c => c.Employee)
            .WithMany(e => e.Cases)
            .HasForeignKey(c => c.AssignedEmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Cases", SchemaNames.Case);
    }
}
