using LOMs.Domain.Cases.ClientFiles;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagement;

public class ClientFileConfiguration : IEntityTypeConfiguration<ClientFile>
{
    public void Configure(EntityTypeBuilder<ClientFile> builder)
    {
        builder.HasKey(cf => cf.Id)
               .IsClustered(false);
        builder.Property(cf => cf.Id)
               .ValueGeneratedNever();

        builder.Property(cf => cf.HijriYear)
               .IsRequired();

        builder.Property(cf => cf.CourtTypeCode)
               .IsRequired()
               .HasMaxLength(10)
               .IsUnicode(false);

        builder.Property(cf => cf.ClientId)
               .IsRequired();
        builder.HasIndex(cf => cf.ClientId)
               .HasDatabaseName("IX_ClientFiles_ClientId");
        builder.HasIndex(cf => cf.CourtTypeCode)
               .HasDatabaseName("IX_ClientFiles_CourtTypeCode");

        builder.HasOne(cf => cf.Client)
               .WithMany(c => c.ClientFiles)
               .HasForeignKey(cf => cf.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("ClientFiles", SchemaNames.GetName(DatabaseSchema.Case));
    }
}
