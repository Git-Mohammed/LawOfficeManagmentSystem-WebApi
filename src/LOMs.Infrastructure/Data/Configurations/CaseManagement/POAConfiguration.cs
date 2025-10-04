using LOMs.Domain.POAs;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

public class POAConfiguration : IEntityTypeConfiguration<POA>
{
    public void Configure(EntityTypeBuilder<POA> builder)
    {
        builder.HasKey(p => p.Id)
               .IsClustered(false);
        builder.Property(p => p.Id)
               .ValueGeneratedNever();

        builder.Property(p => p.CaseId)
               .IsRequired();
        builder.HasIndex(p => p.CaseId)
               .HasDatabaseName("IX_POAs_CaseId");
        builder.HasOne(p => p.Case)
               .WithMany(c => c.POAs)
               .HasForeignKey(p => p.CaseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.POANumber)
               .IsRequired()
               .HasMaxLength(255)
               .IsUnicode();

        builder.Property(p => p.IssueDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(p => p.IssuingAuthority)
               .IsRequired()
               .HasMaxLength(500)
               .IsUnicode(true);

        builder.ToTable("POAs", SchemaNames.GetName(DatabaseSchema.Case));
    }
}
