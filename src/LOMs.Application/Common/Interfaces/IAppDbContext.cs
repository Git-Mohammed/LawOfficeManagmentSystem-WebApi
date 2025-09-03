
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Customers;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using LOMs.Domain.People.Employees;

namespace LOMs.Application.Common.Interfaces;
public interface IAppDbContext
{
    public DbSet<Customer> Customers { get; }
    public DbSet<Person> People { get; }
    public DbSet<Client> Clients { get; }
    public DbSet<ClientFile> ClientFiles { get; }
    public DbSet<Case> Cases { get; }
    public DbSet<ClientCase> ClientCases { get; }
    public DbSet<Contract> Contracts { get; }
    public DbSet<Employee> Employees { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
};
