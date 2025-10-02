using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.Attachments;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.Contracts;
using LOMs.Domain.Cases.CourtSessions;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Cases.POAs;
using LOMs.Domain.Identity;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Domain.People.Phones;
using LOMs.Domain.POAs;
using LOMs.Domain.Templates;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Data.Seeds;
using LOMs.Infrastructure.Data.Views.ClientFiles;
using LOMs.Infrastructure.Data.Views.Contracts;
using LOMs.Infrastructure.Enums;
using LOMs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LOMs.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options), IAppDbContext
{
    // People
    public DbSet<Person> People => Set<Person>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Employee> Employees => Set<Employee>();

    // Cases
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<ClientCase> ClientCases => Set<ClientCase>();
    public DbSet<ClientFile> ClientFiles => Set<ClientFile>();
    public DbSet<ClientFileWithFileNumber> ClientFilesWithFileNumber => Set<ClientFileWithFileNumber>();
    public DbSet<Judgment> Judgments => Set<Judgment>();
    public DbSet<JudgmentFile> JudgmentFiles => Set<JudgmentFile>();
    public DbSet<CourtType> CourtTypes => Set<CourtType>();
    public DbSet<CourtSession> CourtSessions => Set<CourtSession>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ContractFile> ContractFiles => Set<ContractFile>();
    public DbSet<ContractsWithContractNumber> ContractsWithContractNumber => Set<ContractsWithContractNumber>();

    public DbSet<POA> POAs => Set<POA>();
    public DbSet<POAFile> POAFiles => Set<POAFile>();
    public DbSet<Phone> Phones => Set<Phone>();


    // Identity
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Attachment> Attachments => Set<Attachment>();

    public DbSet<Template> Templates => Set<Template> ();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureViews(builder);
        ConfigureIdentityTables(builder);
                    // Seed immutable CourtType lookups
        builder.Entity<CourtType>()
                .HasData(SeedData.CourtTypes.Data);
    }

    private static void ConfigureViews(ModelBuilder builder)
    {
        // Map the ClientFiles view (read-only, no primary key)
        builder.Entity<ClientFileWithFileNumber>()
            .ToView(ClientFilesWithFileNumberView.Name, ClientFilesWithFileNumberView.Schema)
            .HasNoKey();

        // Map the Contracts view (read-only, no primary key)
        builder.Entity<ContractsWithContractNumber>()
            .ToView(ContractsWithContractNumberView.Name, ContractsWithContractNumberView.Schema)
            .HasNoKey();
    }


    private static void ConfigureIdentityTables(ModelBuilder builder)
    {
        var schema = SchemaNames.GetName(DatabaseSchema.Identity);

        builder.Entity<ApplicationUser>().ToTable("Users", schema);
        builder.Entity<ApplicationRole>().ToTable("Roles", schema);
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schema);
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schema);
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", schema);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", schema);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", schema);
    }
}
