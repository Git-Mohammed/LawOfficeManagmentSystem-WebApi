using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOMs.Domain.Cases.Contracts;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment
{
    internal class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(c => c.Id).IsClustered(false);

            builder.Property(c => c.ContractNumber)
                .IsRequired();

            builder.Property(c => c.Type)
                .HasConversion<int>() // Store enum as int
                .HasComment("نوع العقد: 1 = محدد، 2 = غير محدد")
                .IsRequired();

            builder.Ignore(c => c.TotalAmount);

            builder.Property(c => c.IssuedOn);

            builder.Property(c => c.ExpiresOn);

            builder.Property(c => c.BaseAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.InitialPayment)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.FilePath)
                .IsRequired();

            builder.Property(c => c.IsAssigned)
                .IsRequired();

            builder.Property(cf => cf.HijriYear)
                .IsRequired();

            builder.Property(cf => cf.OrderNumber)
                .IsRequired();

            builder.Property(cf => cf.CourtTypeCode)
                .IsRequired();
        }
    }
}
