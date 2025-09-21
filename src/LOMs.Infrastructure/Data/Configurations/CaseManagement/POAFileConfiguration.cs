using LOMs.Domain.Cases.POAs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.CaseManagment
{
    internal class POAFileConfiguration : IEntityTypeConfiguration<POAFile>
    {
        public void Configure(EntityTypeBuilder<POAFile> builder)
        {

            builder.HasKey(cf => cf.Id).IsClustered(false);

            builder.Property(cf => cf.POAId)
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

            builder.HasOne(x => x.POA)
                .WithMany(x => x.POAFiles)
                .HasForeignKey(cf => cf.POAId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cf => cf.POAId);
            builder.ToTable("POAFiles", "FileManagement");
        }
    }
}
