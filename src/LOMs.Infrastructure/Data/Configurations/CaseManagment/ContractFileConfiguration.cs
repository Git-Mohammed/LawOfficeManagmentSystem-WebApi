using LOMs.Domain.Cases.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment
{
    internal class ContractFileConfiguration : IEntityTypeConfiguration<ContractFile>
    {
        public void Configure(EntityTypeBuilder<ContractFile> builder)
        {
           builder.HasKey(cf => cf.Id).IsClustered(false);

            builder.Property(cf => cf.FileName)
                .IsRequired(false);

            builder.Property(cf => cf.FilePath)
                .IsRequired();

            builder.HasOne(x => x.Contract)
                .WithMany(c => c.ContractFiles)
                .HasForeignKey(cf => cf.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
