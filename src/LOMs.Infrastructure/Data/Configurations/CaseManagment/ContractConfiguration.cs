using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LOMs.Domain.Cases.Contracts;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment
{
    internal class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ContractNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Type)
                .HasConversion<int>() // Store enum as int
                .HasComment("نوع العقد: 1 = محدد، 2 = غير محدد")

                .IsRequired();

            builder.Property(c => c.IssuedOn)
                .HasColumnType("date");

            builder.Property(c => c.ExpiresOn)
                .HasColumnType("date");

            builder.Property(c => c.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.InitialPayment)
                .HasColumnType("decimal(18,2)")
                .IsRequired(); // Required even if zero

            builder.Property(c => c.FilePath)
                .IsRequired()
                .HasMaxLength(255); // Mandatory attachment

            builder.Property(c => c.IsAssigned)
                .IsRequired();
        }
    }
}
