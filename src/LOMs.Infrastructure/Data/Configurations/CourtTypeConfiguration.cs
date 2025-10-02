using LOMs.Domain.Cases.CourtTypes;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

public class CourtTypeConfiguration : IEntityTypeConfiguration<CourtType>
{
    public void Configure(EntityTypeBuilder<CourtType> builder)
    {
        builder.ToTable("CourtTypes", SchemaNames.GetName(DatabaseSchema.Case));

        builder.HasKey(ct => ct.Id)
               .IsClustered(false);
        builder.Property(ct => ct.Id)
               .ValueGeneratedNever();

        builder.Property(ct => ct.Code)
               .IsRequired()
               .HasMaxLength(10)
               .IsUnicode(false);
        builder.HasIndex(ct => ct.Code)
               .IsUnique()
               .HasDatabaseName("UX_CourtTypes_Code");

        builder.Property(ct => ct.Name)
               .IsRequired()
               .HasMaxLength(255)
               .IsUnicode(true);
        builder.HasIndex(ct => ct.Name)
               .HasDatabaseName("IX_CourtTypes_Name");

        builder.HasMany(ct => ct.Cases)
               .WithOne(c => c.CourtType)
               .HasForeignKey(c => c.CourtTypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
