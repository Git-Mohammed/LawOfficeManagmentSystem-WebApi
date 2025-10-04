using LOMs.Domain.Cases.Contracts;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

internal class ContractFileConfiguration : IEntityTypeConfiguration<ContractFile>
{
    public void Configure(EntityTypeBuilder<ContractFile> builder)
    {
        builder.HasKey(cf => cf.Id)
               .IsClustered(false);
        builder.Property(cf => cf.Id)
               .ValueGeneratedNever();

        builder.Property(cf => cf.ContractId)
               .IsRequired();
        builder.HasIndex(cf => cf.ContractId)
               .HasDatabaseName("IX_ContractFiles_ContractId");

        builder.Property(cf => cf.Filename)
               .IsRequired()
               .HasMaxLength(200)
               .IsUnicode(false);

        builder.Property(cf => cf.Filepath)
               .IsRequired()
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(cf => cf.Extension)
               .HasMaxLength(8)
               .IsUnicode(false);

        builder.Property(cf => cf.ContentType)
               .HasMaxLength(100)
               .IsUnicode(false);

        builder.Property(cf => cf.FileType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(30)
               .IsUnicode(false);
        builder.HasIndex(cf => new { cf.ContractId, cf.FileType })
               .HasDatabaseName("IX_ContractFiles_ContractId_FileType");

        builder.HasOne(cf => cf.Contract)
               .WithMany(c => c.ContractFiles)
               .HasForeignKey(cf => cf.ContractId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("ContractFiles", SchemaNames.GetName(DatabaseSchema.File));
    }
}
