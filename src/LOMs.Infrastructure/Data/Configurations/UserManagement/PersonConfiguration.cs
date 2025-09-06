using LOMs.Domain.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id).IsClustered(false);

        builder.Property(p => p.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(p => p.NationalId)
               .IsRequired()
               .HasMaxLength(11);

        builder.Property(p => p.BirthDate)
               .IsRequired();

        builder.Property(p => p.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(p => p.Address)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(p => p.CountryCode).HasConversion<string>()
               .IsRequired();
        
        builder.HasIndex(p => p.NationalId);
        builder.ToTable("People","UserManagement");
    }
}
