using LOMs.Domain.People.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOMs.Infrastructure.Data.Configurations.UserManagement;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id).IsClustered(false);

        builder.HasOne(c => c.Person)
               .WithOne(c => c.Client)
               .HasForeignKey<Client>(x => x.PersonId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
