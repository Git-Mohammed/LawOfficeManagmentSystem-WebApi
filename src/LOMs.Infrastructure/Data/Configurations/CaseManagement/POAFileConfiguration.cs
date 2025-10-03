using LOMs.Domain.Cases.POAs;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

internal class POAFileConfiguration : IEntityTypeConfiguration<POAFile>
{
    public void Configure(EntityTypeBuilder<POAFile> builder)
    {
        builder.HasKey(cf => cf.Id)
               .IsClustered(false);
        builder.Property(cf => cf.Id)
               .ValueGeneratedNever();

        builder.Property(cf => cf.POAId)
               .IsRequired();
        builder.HasIndex(cf => cf.POAId)
               .HasDatabaseName("IX_POAFiles_POAId");

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
        builder.HasIndex(cf => new { cf.POAId, cf.FileType })
               .HasDatabaseName("IX_POAFiles_POAId_FileType");

        builder.HasOne(cf => cf.POA)
               .WithMany(p => p.POAFiles)
               .HasForeignKey(cf => cf.POAId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("POAFiles", SchemaNames.GetName(DatabaseSchema.File));
    }
}
