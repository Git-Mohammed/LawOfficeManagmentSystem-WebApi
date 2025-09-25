using LOMs.Domain.Cases;
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
using Microsoft.EntityFrameworkCore;

public interface IAppDbContext
{
    public DbSet<Person> People { get; }
    public DbSet<Client> Clients { get; }
    public DbSet<ClientFile> ClientFiles { get; }
    public DbSet<Case> Cases { get; }
    public DbSet<ClientCase> ClientCases { get; }
    public DbSet<Contract> Contracts { get; }
    public DbSet<ContractFile> ContractFiles { get; }
    public DbSet<Phone> Phones { get; }

    public DbSet<POA> POAs { get; }
    public DbSet<POAFile> POAFiles { get; }
    public DbSet<CourtType> CourtTypes { get; }
    public DbSet<CourtSession> CourtSessions { get; }
    public DbSet<Employee> Employees { get; }
    public DbSet<RefreshToken> RefreshTokens { get; }
    public DbSet<Judgment> Judgments { get; }
    public DbSet<JudgmentFile> JudgmentFiles { get; }

    public DbSet<ClientFileWithFileNumber> ClientFilesWithFileNumber { get; }
    public DbSet<ContractsWithContractNumber> ContractsWithContractNumber { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
