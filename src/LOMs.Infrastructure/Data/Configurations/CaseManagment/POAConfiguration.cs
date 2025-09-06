using LOMs.Domain.POAs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment;

public class POAConfiguration : IEntityTypeConfiguration<POA>
{
    public void Configure(EntityTypeBuilder<POA> builder)
    {
        builder.HasKey(p => p.Id).IsClustered(false);

        builder.HasOne(p => p.Case)
            .WithMany(p => p.POAs)
            .HasForeignKey(p => p.CaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.POANumber)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Official POA number as stated in the document");

        builder.Property(p => p.IssueDate)
            .IsRequired()
            .HasComment("Date the POA was issued");

        builder.Property(p => p.IssuingAuthority)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("The authority that issued the POA, such as a court or notary");

        builder.Property(p => p.AttachmentPath)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Path or URL to the attached POA document");
    }
}
