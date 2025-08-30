
using LOMs.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LOMs.Application.Common.Interfaces;
public interface IAppDbContext
{
    public DbSet<Customer> Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
};
