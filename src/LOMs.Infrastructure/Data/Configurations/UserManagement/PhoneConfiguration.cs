using LOMs.Domain.People;
using LOMs.Domain.People.Phones;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.HasKey(p => p.Id)
               .IsClustered(false);
        builder.Property(p => p.Id)
               .ValueGeneratedNever();

        builder.Property(p => p.PersonId)
               .IsRequired();
        builder.HasIndex(p => p.PersonId)
               .HasDatabaseName("IX_Phones_PersonId");

        builder.Property(p => p.Number)
               .IsRequired()
               .HasMaxLength(20)
               .IsUnicode(false);

        builder.Property(p => p.Code)
               .IsRequired()
               .HasMaxLength(10)
               .IsUnicode(false);

        builder.HasOne(p => p.Person)
               .WithMany(p => p.Phones)
               .HasForeignKey(p => p.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Phones", SchemaNames.GetName(DatabaseSchema.User));
    }
}
