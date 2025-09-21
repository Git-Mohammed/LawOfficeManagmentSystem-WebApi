using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Identity;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Domain.POAs;
using LOMs.Infrastructure.Enums;
using LOMs.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Cases.POAs;

namespace LOMs.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser,ApplicationRole,string>(options), IAppDbContext
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Case> Cases => Set<Case>();
    public DbSet<ClientCase> ClientCases => Set<ClientCase>();

    public DbSet<ClientFile> ClientFiles => Set<ClientFile>();

    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ContractFile> ContractFiles => Set<ContractFile>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<POA> POAs => Set<POA>();
    public DbSet<POAFile> POAFiles => Set<POAFile>();
    public DbSet<CourtType> CourtTypes => Set<CourtType>();
    public DbSet<CourtSession> CourtSessions => Set<CourtSession>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Judgment> Judgments => Set<Judgment>();
    public DbSet<JudgmentFile> JudgmentFiles => Set<JudgmentFile>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        var identitySchema = DatabaseSchema.Identity.ToString();
        #region Rename Identity Tables Names
        builder.Entity<ApplicationUser>().ToTable("Users", identitySchema);
        builder.Entity<ApplicationRole>().ToTable("Roles", identitySchema);
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", identitySchema);
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", identitySchema);
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", identitySchema);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", identitySchema);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", identitySchema);
        #endregion
    }
}