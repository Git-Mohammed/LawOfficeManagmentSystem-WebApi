using LOMs.Infrastructure.Data.Views.Registry;
using LOMs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using LOMs.Infrastructure.Data.Seeds;
using LOMs.Infrastructure.Constants;
using LOMs.Domain.Identity;

namespace LOMs.Infrastructure.Data;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly AppDbContext                         _context;
    private readonly RoleManager<ApplicationRole>         _roleManager;
    private readonly UserManager<ApplicationUser>         _userManager;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        AppDbContext context,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _logger     = logger;
        _context    = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Applies all pending EF Core migrations.
    /// </summary>
    public async Task InitialiseAsync()
    {
        try
        {
            _logger.LogInformation("Applying pending migrations (if any)...");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }

    /// <summary>
    /// Creates or updates all registered SQL views.
    /// </summary>
    public async Task EnsureViewsCreatedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Ensuring SQL views are created or updated...");

        foreach (var view in SqlViewRegistry.All)
        {
            var schemaName   = SchemaNames.GetName(view.Schema);
            var fullViewName = $"{schemaName}.[{view.ViewName}]";

            try
            {
                await _context.Database.ExecuteSqlRawAsync(view.Sql, cancellationToken);
                _logger.LogInformation("View {ViewName} created/updated.", fullViewName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create/update view {ViewName}.", fullViewName);
                throw;
            }
        }
    }

    /// <summary>
    /// Runs idempotent seed logic for roles, lookup data and default user.
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {

        if (_roleManager != null)
        {
            var roles = new[]
            {
                Role.GeneralManager.ToString(),
                Role.ExecutiveManager.ToString(),
                Role.Consultant.ToString(),
                Role.Lawyer.ToString()
           };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Failed to create role '{roleName}': {errors}");
                    }
                }
            }
        }
        // --- Employees and Identity Users Seed Data ---
        if (!_context.Employees.Any())
        {
            var employeesData = new[]
            {
        new
        {
            PersonId = Guid.Parse("44444444-dd2d-4eee-ffff-444443244443"),
            Name = "عبدالعزيز حسن",
            NationalId = "0000000000",
            Nationality = "SA",
            BirthDate = new DateOnly(2006, 3, 3),
            Phone = "547020151",
            Address = "Abha, Saudi Arabia",
            Email = "abdulaziz.ksa939@gmail.com",
            Role = Role.GeneralManager,
            Password = "Admin123@"
        },
    };

        // Default admin user
        var identityUser = await _userManager.FindByNameAsync(SeedData.DefaultUser.NationalId);
        if (identityUser == null)
        {
                identityUser = new ApplicationUser
            {
                UserName       = SeedData.DefaultUser.NationalId,
                Email          = SeedData.DefaultUser.Email,
                EmailConfirmed = true
            };

                var result = await _userManager.CreateAsync(identityUser, employeesData[0].Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning($"Failed to create user '{employeesData[0].NationalId}': {errors}");
                    continue;
                }

                await _userManager.AddToRoleAsync(identityUser, employeesData[0].Role.ToString());
                employeeResult.Value.AssignUser(identityUser.Id);

                _context.People.Add(personResult.Value);
                _context.Employees.Add(employeeResult.Value);
            }

            await _context.SaveChangesAsync();
        }

    //    // --- Clients Seed Data ---
    //    if (!_context.Clients.Any())
    //    {
    //        var clients = new List<Client>();

    //        var phone1 = new List<Phone>
    //{
    //    Phone.Create(Guid.NewGuid(), "501234567", "966").Value
    //};

    //        var person1Result = Person.Create(
    //            Guid.Parse("11111111-aaaa-4bbb-cccc-111111111111"),
    //            "Ahmed Al-Harbi",
    //            "1234567890",
    //            "SA",
    //            new DateOnly(1990, 5, 12),
    //            "Riyadh, Saudi Arabia",
    //            phone1
    //        );
    //        if (person1Result.IsSuccess)
    //        {
    //            var client1 = Client.Create(Guid.Parse("aaaa1111-bbbb-4ccc-dddd-111111111111"), person1Result.Value).Value;
    //            _context.People.Add(person1Result.Value);
    //            _context.Clients.Add(client1);
    //        }

    //        var phone2 = new List<Phone>
    //{
    //    Phone.Create(Guid.NewGuid(), "502345678", "966").Value
    //};

    //        var person2Result = Person.Create(
    //            Guid.Parse("22222222-bbbb-4ccc-dddd-222222222222"),
    //            "Fatimah Al-Qahtani",
    //            "2345678901",
    //            "SA",
    //            new DateOnly(1985, 11, 3),
    //            "Jeddah, Saudi Arabia",
    //            phone2
    //        );
    //        if (person2Result.IsSuccess)
    //        {
    //            var client2 = Client.Create(Guid.Parse("bbbb2222-cccc-4ddd-eeee-222222222222"), person2Result.Value).Value;
    //            _context.People.Add(person2Result.Value);
    //            _context.Clients.Add(client2);
    //        }

    //        var phone3 = new List<Phone>
    //{
    //    Phone.Create(Guid.NewGuid(), "503456789", "966").Value,
    //    Phone.Create(Guid.NewGuid(), "503456789", "966").Value
    //};

    //        var person3Result = Person.Create(
    //            Guid.Parse("33333333-cccc-4ddd-eeee-333333333333"),
    //            "Mohammed Al-Shehri",
    //            "3456789012",
    //            "SA",
    //            new DateOnly(1992, 2, 20),
    //            "Dammam, Saudi Arabia",
    //            phone3
    //        );
    //        if (person3Result.IsSuccess)
    //        {
    //            var client3 = Client.Create(Guid.NewGuid(), person3Result.Value).Value;
    //            _context.People.Add(person3Result.Value);
    //            _context.Clients.Add(client3);
    //        }

    //        await _context.SaveChangesAsync();
    //    }



    //    // --- Cases Seed Data ---
    //    if (!_context.Cases.Any())
    //    {
    //        _logger.LogInformation("Seeding initial cases...");

    //        var seededCases = new List<Case>();
    //        var seededClientCases = new List<ClientCase>();
    //        var seededClientFiles = new List<ClientFile>();

    //        // Ensure required court type and employee exist
    //        var defaultCourtType = await _context.CourtTypes.FirstOrDefaultAsync();
    //        var defaultEmployee = await _context.Employees.FirstOrDefaultAsync();

    //        if (defaultCourtType is null || defaultEmployee is null)
    //        {
    //            _logger.LogWarning("Cannot seed cases: missing CourtType or Employee.");
    //            return;
    //        }

    //        // Example Case Seeds
    //        var caseSeeds = new[]
    //        {
    //    new
    //    {
    //        Id = Guid.Parse("30000000-cccc-dddd-eeee-000000000003"),
    //        CaseNumber = "CASE-001",
    //        Subject = "Property Dispute",
    //        PartyRole = PartyRole.Plaintiff,
    //        Requests = "Recover ownership of disputed land.",
    //        ReviewDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
    //        Status = CaseStatus.Processing,
    //        Opinion = "Strong case with supporting documents.",
    //        ClientId = Guid.Parse("aaaa1111-bbbb-4ccc-dddd-111111111111") // seeded client
    //    },
    //    new
    //    {
    //        Id = Guid.Parse("40000000-dddd-eeee-ffff-000000000004"),
    //        CaseNumber = "CASE-002",
    //        Subject = "Contract Breach",
    //        PartyRole = PartyRole.Defendant,
    //        Requests = "Dismiss breach claims and recover damages.",
    //        ReviewDate = DateOnly.FromDateTime(DateTime.Today.AddDays(45)),
    //        Status = CaseStatus.Draft,
    //        Opinion = "Requires further evidence from client.",
    //        ClientId = Guid.Parse("bbbb2222-cccc-4ddd-eeee-222222222222") // seeded client
    //    },
    //    new
    //    {
    //        Id = Guid.Parse("50000000-eeee-ffff-aaaa-000000000005"),
    //        CaseNumber = "CASE-003",
    //        Subject = "Inheritance Dispute",
    //        PartyRole = PartyRole.Plaintiff,
    //        Requests = "Claim rightful inheritance share.",
    //        ReviewDate = DateOnly.FromDateTime(DateTime.Today.AddDays(60)),
    //        Status = CaseStatus.Pending,
    //        Opinion = "Awaiting notarized documents.",
    //        ClientId = Guid.Parse("cccc3333-dddd-4eee-ffff-333333333333") // seeded client
    //    }
    //};

    //        foreach (var seed in caseSeeds)
    //        {
    //            // Create case entity
    //            var caseResult = Case.Create(
    //                id: seed.Id,
    //                courtTypeId: defaultCourtType.Id,
    //                caseNumber: seed.CaseNumber,
    //                caseSubject: seed.Subject,
    //                partyRole: seed.PartyRole,
    //                clientRequests: seed.Requests,
    //                estimatedReviewDate: seed.ReviewDate,
    //                status: seed.Status,
    //                lawyerOpinion: seed.Opinion,
    //                assignedEmployeeId: defaultEmployee.Id
    //            );

    //            if (caseResult.IsError)
    //            {
    //                _logger.LogWarning("Failed to seed case {CaseNumber}: {Errors}",
    //                    seed.CaseNumber, string.Join(", ", caseResult.Errors));
    //                continue;
    //            }

    //            var caseEntity = caseResult.Value;
    //            seededCases.Add(caseEntity);

    //            // Ensure client exists
    //            var client = await _context.Clients
    //                .Include(c => c.ClientFiles)
    //                .FirstOrDefaultAsync(c => c.Id == seed.ClientId);

    //            if (client is null)
    //            {
    //                _logger.LogWarning("Skipping case {CaseNumber}: missing client {ClientId}",
    //                    seed.CaseNumber, seed.ClientId);
    //                continue;
    //            }

    //            // Create/Find client file for the case
    //            var currentHijriYear = HijriDateConverter.GetFullHijriYear();
    //            var clientFile = client.ClientFiles
    //                .FirstOrDefault(cf => cf.CourtTypeCode == defaultCourtType.Code && cf.HijriYear == currentHijriYear);

    //            if (clientFile is null)
    //            {
    //                var clientFileResult = ClientFile.Create(client.Id, defaultCourtType, (short)currentHijriYear);
    //                if (clientFileResult.IsError)
    //                {
    //                    _logger.LogWarning("Failed to create client file for client {ClientId}", client.Id);
    //                    continue;
    //                }
    //                clientFile = clientFileResult.Value;
    //                //seededClientFiles.Add(clientFile);
    //            }

    //            // Link client to case
    //            var clientCase = new ClientCase(caseEntity.Id, client.Id, clientFile.Id);
    //            seededClientCases.Add(clientCase);
    //                 _context.Cases.Add(caseEntity);
    //                 _context.ClientFiles.Add(clientFile);
    //                 _context.ClientCases.Add(clientCase);
    //            await _context.SaveChangesAsync();


    //            _logger.LogInformation("Seeded case {CaseNumber} with client {ClientId}", seed.CaseNumber, client.Id);
    //        }

    //        //// Save all at once
    //        //if (seededCases.Any())
    //        //    await _context.Cases.AddRangeAsync(seededCases);

    //        //if (seededClientFiles.Any())
    //        //    await _context.ClientFiles.AddRangeAsync(seededClientFiles);

    //        //if (seededClientCases.Any())
    //        //    await _context.ClientCases.AddRangeAsync(seededClientCases);

    //        await _context.SaveChangesAsync();
    //        _logger.LogInformation("Seeding of cases completed successfully.");
    //    }


    }
}

/// <summary>
/// Extension to wire up initialization on application startup.
/// </summary>
public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitialiseAsync();            // apply migrations
        await initializer.EnsureViewsCreatedAsync();     // create/update views
        await initializer.SeedAsync();                  // seed roles, user, lookups
    }
}
