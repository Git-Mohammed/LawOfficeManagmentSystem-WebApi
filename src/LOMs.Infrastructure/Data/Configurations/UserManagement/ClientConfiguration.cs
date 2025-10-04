using LOMs.Domain.People.Clients;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id)
               .IsClustered(false);
        builder.Property(c => c.Id)
               .ValueGeneratedNever();

        builder.Property(c => c.PersonId)
               .IsRequired();
        builder.HasIndex(c => c.PersonId)
               .IsUnique()
               .HasDatabaseName("UX_Clients_PersonId");

        builder.HasOne(c => c.Person)
               .WithOne(p => p.Client)
               .HasForeignKey<Client>(c => c.PersonId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Clients", SchemaNames.GetName(DatabaseSchema.User));
    }
}
