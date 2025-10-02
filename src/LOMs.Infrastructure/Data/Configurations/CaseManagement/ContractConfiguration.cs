using LOMs.Domain.Cases.Contracts;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

internal class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id)
               .IsClustered(false);
        builder.Property(c => c.Id)
               .ValueGeneratedNever();

        builder.Property(c => c.Type)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsUnicode(false)
               .IsRequired();

        builder.Ignore(c => c.TotalAmount);

        builder.Property(c => c.IssuedOn)
               .HasColumnType("date");

        builder.Property(c => c.ExpiresOn)
               .HasColumnType("date");

        builder.Property(c => c.BaseAmount)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(c => c.InitialPayment)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(c => c.TaxAmount)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(c => c.IsAssigned)
               .IsRequired();

        builder.Property(c => c.HijriYear)
               .HasColumnType("smallint")
               .IsRequired();
        builder.HasIndex(c => c.HijriYear)
               .HasDatabaseName("IX_Contracts_HijriYear");

        builder.Property(c => c.CourtTypeCode)
               .HasMaxLength(10)
               .IsUnicode(false)
               .IsRequired();
        builder.HasIndex(c => c.CourtTypeCode)
               .HasDatabaseName("IX_Contracts_CourtTypeCode");

        builder.ToTable("Contracts", SchemaNames.GetName(DatabaseSchema.Finance));
    }
}
