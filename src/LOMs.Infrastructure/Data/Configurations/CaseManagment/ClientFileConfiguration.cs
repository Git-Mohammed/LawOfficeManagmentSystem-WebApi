using LOMs.Domain.Cases.ClientFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment;

public class ClientFileConfiguration : IEntityTypeConfiguration<ClientFile>
{
    public void Configure(EntityTypeBuilder<ClientFile> builder)
    {
        builder.HasKey(cf => cf.Id).IsClustered(false);

        builder.Property(cf => cf.FileNumber)
            .IsRequired();


        builder.HasOne(cf => cf.Client)
            .WithMany(c => c.ClientFiles)
            .HasForeignKey(cf => cf.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(cf => cf.Cases)
            .WithOne(c => c.ClientFile)
            .HasForeignKey(c => c.ClientFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
