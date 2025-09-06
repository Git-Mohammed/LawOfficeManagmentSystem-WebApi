using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Customers;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Domain.POAs;
using LOMs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser,ApplicationRole,string>(options), IAppDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Case> Cases => Set<Case>();
    public DbSet<ClientCase> ClientCases => Set<ClientCase>();

    public DbSet<ClientFile> ClientFiles => Set<ClientFile>();

    public DbSet<Contract> Contracts => Set<Contract>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<POA> POAs => Set<POA>();

    public DbSet<CourtType> CourtTypes => Set<CourtType>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        #region Rename Identity Tables Names
        builder.Entity<ApplicationUser>().ToTable("Users", "Identity");
        builder.Entity<ApplicationRole>().ToTable("Roles", "Identity");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");
        #endregion
    }
}