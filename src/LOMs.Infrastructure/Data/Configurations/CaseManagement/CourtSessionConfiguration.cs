using LOMs.Domain.Cases.CourtSessions;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CourtSessionConfiguration : IEntityTypeConfiguration<CourtSession>
{
    public void Configure(EntityTypeBuilder<CourtSession> builder)
    {
        builder.ToTable("CourtSessions", SchemaNames.GetName(DatabaseSchema.Case));

        builder.HasKey(cs => cs.Id).IsClustered(false);

        builder.Property(cs => cs.CaseId)
               .IsRequired();

        builder.Property(cs => cs.AssignedEmployeeId)
               .IsRequired();

        builder.Property(cs => cs.ScheduledAt)
               .IsRequired();

        builder.Property(cs => cs.AssignedTasks)
               .IsRequired();

        builder.HasOne(cs => cs.Case)
               .WithMany(c => c.CourtSessions)
               .HasForeignKey(cs => cs.CaseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cs => cs.AssignedEmployee)
               .WithMany()
               .HasForeignKey(cs => cs.AssignedEmployeeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
