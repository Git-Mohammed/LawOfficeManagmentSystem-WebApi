using LOMs.Domain.Cases.ClientFiles;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment;

public class ClientFileConfiguration : IEntityTypeConfiguration<ClientFile>
{
    public void Configure(EntityTypeBuilder<ClientFile> builder)
    {
        builder.HasKey(cf => cf.Id).IsClustered(false);


        builder.Property(cf => cf.HijriYear)
               .IsRequired();

        builder.Property(cf => cf.CourtTypeCode)
                .IsRequired();

        builder.HasOne(cf => cf.Client)
            .WithMany(c => c.ClientFiles)
            .HasForeignKey(cf => cf.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("ClientFiles", SchemaNames.GetName(DatabaseSchema.Case));

    }
}
