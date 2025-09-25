using LOMs.Domain.Cases.Contracts;
using LOMs.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment
{
    internal class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            // Primary key (non-clustered to allow flexibility for indexing strategies)
            builder.HasKey(c => c.Id).IsClustered(false);

            // Store ContractType enum as int in the database
            builder.Property(c => c.Type)
                .HasConversion<int>()
                .HasComment("Contract type: 1 = Fixed Term, 2 = Open Term")
                .IsRequired();

            // TotalAmount is a calculated property, not mapped to the database
            builder.Ignore(c => c.TotalAmount);

            // Optional issue and expiry dates (depending on contract type)
            builder.Property(c => c.IssuedOn);
            builder.Property(c => c.ExpiresOn);

            // Monetary values with precision
            builder.Property(c => c.BaseAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.InitialPayment)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.TaxAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Assignment flag
            builder.Property(c => c.IsAssigned)
                .IsRequired();

            // Hijri year is required
            builder.Property(c => c.HijriYear)
                .IsRequired();

            // Court type code is required
            builder.Property(c => c.CourtTypeCode)
                .IsRequired();

            // Map to Contracts table in Finance schema
            builder.ToTable("Contracts", SchemaNames.Finance);
        }
    }
}
