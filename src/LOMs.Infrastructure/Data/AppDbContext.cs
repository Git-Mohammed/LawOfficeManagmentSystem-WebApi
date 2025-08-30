using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Customers;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}