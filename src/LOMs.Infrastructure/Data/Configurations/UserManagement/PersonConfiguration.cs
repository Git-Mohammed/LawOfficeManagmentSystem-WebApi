using LOMs.Domain.People;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id)
               .IsClustered(false);
        builder.Property(p => p.Id)
               .ValueGeneratedNever();

        builder.Property(p => p.FullName)
               .IsRequired()
               .HasMaxLength(255)
               .IsUnicode(true);

        builder.Property(p => p.NationalId)
               .IsRequired()
               .HasMaxLength(11)
               .IsUnicode(false);
        builder.HasIndex(p => p.NationalId)
               .IsUnique()
               .HasDatabaseName("UX_People_NationalId");

        builder.Property(p => p.BirthDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(p => p.Address)
               .IsRequired()
               .HasMaxLength(250)
               .IsUnicode(true);

        builder.Property(p => p.CountryCode)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(2)
               .IsUnicode(false);
        builder.HasIndex(p => p.CountryCode)
               .HasDatabaseName("IX_People_CountryCode");

        builder.ToTable("People", SchemaNames.GetName(DatabaseSchema.User));
    }
}
