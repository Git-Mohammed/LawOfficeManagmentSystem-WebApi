
using LOMs.Domain.Customers;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using Microsoft.EntityFrameworkCore;
using LOMs.Domain.People.Employees;

namespace LOMs.Application.Common.Interfaces;
public interface IAppDbContext
{
    public DbSet<Customer> Customers { get; }
    public DbSet<Person> People { get; }
    public DbSet<Client> Clients { get; }
    public DbSet<Employee> Employees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
};
