using LOMs.Domain.Cases.CourtTypes;
using LOMs.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CourtTypeConfiguration : IEntityTypeConfiguration<CourtType>
{
    public void Configure(EntityTypeBuilder<CourtType> builder)
    {
        builder.ToTable("CourtTypes", SchemaNames.Case);

        builder.HasKey(ct => ct.Id).IsClustered(false);

        builder.Property(ct => ct.Code)
               .IsRequired();

        builder.Property(ct => ct.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasMany(x => x.Cases)
            .WithOne(x => x.CourtType)
            .HasForeignKey(x => x.CourtTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}
