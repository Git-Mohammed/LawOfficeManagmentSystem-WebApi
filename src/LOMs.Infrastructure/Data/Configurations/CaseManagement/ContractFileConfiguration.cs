using LOMs.Domain.Cases.Contracts;
using LOMs.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment
{
    internal class ContractFileConfiguration : IEntityTypeConfiguration<ContractFile>
    {
        public void Configure(EntityTypeBuilder<ContractFile> builder)
        {

            builder.HasKey(cf => cf.Id).IsClustered(false);

            builder.Property(cf => cf.ContractId)
                .IsRequired();

            builder.Property(cf => cf.Filename)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(cf => cf.Filepath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(cf => cf.Extension)
                .HasMaxLength(10);

            builder.Property(cf => cf.ContentType)
                .HasMaxLength(100);

            builder.Property(cf => cf.FileType).HasConversion<string>()
                .IsRequired();

            builder.HasOne(x => x.Contract)
                .WithMany(c => c.ContractFiles)
                .HasForeignKey(cf => cf.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cf => cf.ContractId);
            builder.ToTable("ContractFiles", SchemaNames.File);
        }
    }
}
