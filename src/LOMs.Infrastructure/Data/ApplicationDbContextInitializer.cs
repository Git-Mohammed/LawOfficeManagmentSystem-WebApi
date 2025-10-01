
using LOMs.Application.Features.ClientFiles;
using LOMs.Application.Utilities;
using LOMs.Domain.Cases;
using LOMs.Domain.Cases.ClientFiles;
using LOMs.Domain.Cases.CourtTypes;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Identity;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Domain.People.Phones;
using LOMs.Infrastructure.Constants;
using LOMs.Infrastructure.Data.Views;
using LOMs.Infrastructure.Data.Views.Registry;
using LOMs.Infrastructure.Enums;
using LOMs.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace LOMs.Infrastructure.Data;

public class ApplicationDbContextInitializer(
    ILogger<ApplicationDbContextInitializer> logger,
    AppDbContext context,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager

    )
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger = logger;
    private readonly AppDbContext _context = context;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task InitialiseAsync()
    {
        try
        {
            if ( (await _context.Database.GetPendingMigrationsAsync()).Any())
            {
               await _context.Database.MigrateAsync();
            }
         
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }


    public async Task EnsureViewsCreatedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Ensuring SQL views are created or updated...");

        foreach (var view in SqlViewRegistry.All)
        {
            var schemaName = SchemaNames.GetName(view.Schema);
            var fullViewName = $"{schemaName}.[{view.ViewName}]";

            try
            {
                await _context.Database.ExecuteSqlRawAsync(view.Sql, cancellationToken);
                _logger.LogInformation("View {ViewName} created or updated successfully.", fullViewName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create or update SQL view {ViewName}.", fullViewName);
                throw;
            }
        }
    }
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
        if (!_context.CourtTypes.Any())
        {
            var courtTypes = new[]
            {
                CourtType.Create(Guid.Parse("b3a1f8b4-1e6a-4b3e-9c3d-0a9f8d8a1b11"), "العامة", 100, "المحكمة العامة").Value,
                CourtType.Create(Guid.Parse("c4f2a9d7-2b7e-4a4f-8b2e-1c2d3e4f5a66"), "الجنائية", 200, "المحكمة الجنائية").Value,
                CourtType.Create(Guid.Parse("d5e3b0c8-3c8f-4c5f-9d3f-2d3e4f5a6b77"), "العمالية", 300, "المحكمة العمالية").Value,
                CourtType.Create(Guid.Parse("e6f4c1d9-4d9f-4d6f-ae4f-3e4f5a6b7c88"), "الأحوال الشخصية", 400, "محكمة الأحوال الشخصية").Value,
                CourtType.Create(Guid.Parse("f7a5d2ea-5eaf-4e7f-bf5f-4f5a6b7c8d99"), "التجارية", 500, "المحكمة التجارية").Value,
                CourtType.Create(Guid.Parse("a8b6e3fb-6fb0-4f8f-cf6f-5a6b7c8d9e10"), "الإدارية", 600, "المحكمة الإدارية ").Value,
                CourtType.Create(Guid.Parse("1816e3fb-6fb0-428f-cf6f-5a7b7c829e10"), "اللجان شبه القضائية", 700, "اللجان شبه القضائية ").Value,

                CourtType.Create(Guid.Parse("b9c7f40c-70c1-4f9f-df7f-6b7c8d9e0f21"), "أخرى", 32000, "أخرى").Value
            };


            _context.CourtTypes.AddRange(courtTypes);
            await _context.SaveChangesAsync();
        }
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
            Name = "arammtech",
            NationalId = "0000000000",
            Nationality = "SA",
            BirthDate = new DateOnly(1988, 7, 15),
            Phone = "504567890",
            Address = "Abha, Saudi Arabia",
            Email = "arammtech@gmail.com",
            Role = Role.GeneralManager,
            Password = "Admin123@"
        },
        new
        {
            PersonId = Guid.Parse("44444444-dddd-4eee-ffff-444444444444"),
            Name = "Salem Al-Dosari",
            NationalId = "4567890123",
            Nationality = "SA",
            BirthDate = new DateOnly(1988, 7, 15),
            Phone = "504567890",
            Address = "Abha, Saudi Arabia",
            Email = "salem.dosari@example.com",
            Role = Role.Lawyer,
            Password = "P@ssword123"
        },
        new
        {
            PersonId = Guid.Parse("55555555-eeee-4fff-aaaa-555555555555"),
            Name = "Noura Al-Fahad",
            NationalId = "5678901234",
            Nationality = "SA",
            BirthDate = new DateOnly(1991, 3, 22),
            Phone = "505678901",
            Address = "Tabuk, Saudi Arabia",
            Email = "noura.fahad@example.com",
            Role = Role.Consultant,
            Password = "P@ssword123"
        }
    };

            foreach (var data in employeesData)
            {
                var phones = new List<Phone>
        {
            Phone.Create(Guid.NewGuid(), data.Phone, "966").Value
        };

                var personResult = Person.Create(data.PersonId, data.Name, data.NationalId, data.Nationality, data.BirthDate, data.Address, phones);
                if (personResult.IsError) continue;

                var employeeResult = Employee.Create(Guid.NewGuid(), data.Email, personResult.Value, data.Role);
                if (employeeResult.IsError) continue;

                var identityUser = new ApplicationUser
                {
                    UserName = data.NationalId,
                    Email = data.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(identityUser, data.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning($"Failed to create user '{data.NationalId}': {errors}");
                    continue;
                }

                await _userManager.AddToRoleAsync(identityUser, data.Role.ToString());
                employeeResult.Value.AssignUser(identityUser.Id);

                _context.People.Add(personResult.Value);
                _context.Employees.Add(employeeResult.Value);
            }

            await _context.SaveChangesAsync();
        }

        // --- Clients Seed Data ---
        if (!_context.Clients.Any())
        {
            var clients = new List<Client>();

            var phone1 = new List<Phone>
    {
        Phone.Create(Guid.NewGuid(), "501234567", "966").Value
    };

            var person1Result = Person.Create(
                Guid.Parse("11111111-aaaa-4bbb-cccc-111111111111"),
                "Ahmed Al-Harbi",
                "1234567890",
                "SA",
                new DateOnly(1990, 5, 12),
                "Riyadh, Saudi Arabia",
                phone1
            );
            if (person1Result.IsSuccess)
            {
                var client1 = Client.Create(Guid.Parse("aaaa1111-bbbb-4ccc-dddd-111111111111"), person1Result.Value).Value;
                _context.People.Add(person1Result.Value);
                _context.Clients.Add(client1);
            }

            var phone2 = new List<Phone>
    {
        Phone.Create(Guid.NewGuid(), "502345678", "966").Value
    };

            var person2Result = Person.Create(
                Guid.Parse("22222222-bbbb-4ccc-dddd-222222222222"),
                "Fatimah Al-Qahtani",
                "2345678901",
                "SA",
                new DateOnly(1985, 11, 3),
                "Jeddah, Saudi Arabia",
                phone2
            );
            if (person2Result.IsSuccess)
            {
                var client2 = Client.Create(Guid.Parse("bbbb2222-cccc-4ddd-eeee-222222222222"), person2Result.Value).Value;
                _context.People.Add(person2Result.Value);
                _context.Clients.Add(client2);
            }

            var phone3 = new List<Phone>
    {
        Phone.Create(Guid.NewGuid(), "503456789", "966").Value,
        Phone.Create(Guid.NewGuid(), "503456789", "966").Value
    };

            var person3Result = Person.Create(
                Guid.Parse("33333333-cccc-4ddd-eeee-333333333333"),
                "Mohammed Al-Shehri",
                "3456789012",
                "SA",
                new DateOnly(1992, 2, 20),
                "Dammam, Saudi Arabia",
                phone3
            );
            if (person3Result.IsSuccess)
            {
                var client3 = Client.Create(Guid.NewGuid(), person3Result.Value).Value;
                _context.People.Add(person3Result.Value);
                _context.Clients.Add(client3);
            }

            await _context.SaveChangesAsync();
        }



        // --- Cases Seed Data ---
        if (!_context.Cases.Any())
        {
            _logger.LogInformation("Seeding initial cases...");

            var seededCases = new List<Case>();
            var seededClientCases = new List<ClientCase>();
            var seededClientFiles = new List<ClientFile>();

            // Ensure required court type and employee exist
            var defaultCourtType = await _context.CourtTypes.FirstOrDefaultAsync();
            var defaultEmployee = await _context.Employees.FirstOrDefaultAsync();

            if (defaultCourtType is null || defaultEmployee is null)
            {
                _logger.LogWarning("Cannot seed cases: missing CourtType or Employee.");
                return;
            }

            // Example Case Seeds
            var caseSeeds = new[]
            {
        new
        {
            Id = Guid.Parse("30000000-cccc-dddd-eeee-000000000003"),
            CaseNumber = "CASE-001",
            Subject = "Property Dispute",
            PartyRole = PartyRole.Plaintiff,
            Requests = "Recover ownership of disputed land.",
            ReviewDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
            Status = CaseStatus.Processing,
            Opinion = "Strong case with supporting documents.",
            ClientId = Guid.Parse("aaaa1111-bbbb-4ccc-dddd-111111111111") // seeded client
        },
        new
        {
            Id = Guid.Parse("40000000-dddd-eeee-ffff-000000000004"),
            CaseNumber = "CASE-002",
            Subject = "Contract Breach",
            PartyRole = PartyRole.Defendant,
            Requests = "Dismiss breach claims and recover damages.",
            ReviewDate = DateOnly.FromDateTime(DateTime.Today.AddDays(45)),
            Status = CaseStatus.Draft,
            Opinion = "Requires further evidence from client.",
            ClientId = Guid.Parse("bbbb2222-cccc-4ddd-eeee-222222222222") // seeded client
        },
        new
        {
            Id = Guid.Parse("50000000-eeee-ffff-aaaa-000000000005"),
            CaseNumber = "CASE-003",
            Subject = "Inheritance Dispute",
            PartyRole = PartyRole.Plaintiff,
            Requests = "Claim rightful inheritance share.",
            ReviewDate = DateOnly.FromDateTime(DateTime.Today.AddDays(60)),
            Status = CaseStatus.Pending,
            Opinion = "Awaiting notarized documents.",
            ClientId = Guid.Parse("cccc3333-dddd-4eee-ffff-333333333333") // seeded client
        }
    };

            foreach (var seed in caseSeeds)
            {
                // Create case entity
                var caseResult = Case.Create(
                    id: seed.Id,
                    courtTypeId: defaultCourtType.Id,
                    caseNumber: seed.CaseNumber,
                    caseSubject: seed.Subject,
                    partyRole: seed.PartyRole,
                    clientRequests: seed.Requests,
                    estimatedReviewDate: seed.ReviewDate,
                    status: seed.Status,
                    lawyerOpinion: seed.Opinion,
                    assignedEmployeeId: defaultEmployee.Id
                );

                if (caseResult.IsError)
                {
                    _logger.LogWarning("Failed to seed case {CaseNumber}: {Errors}",
                        seed.CaseNumber, string.Join(", ", caseResult.Errors));
                    continue;
                }

                var caseEntity = caseResult.Value;
                seededCases.Add(caseEntity);

                // Ensure client exists
                var client = await _context.Clients
                    .Include(c => c.ClientFiles)
                    .FirstOrDefaultAsync(c => c.Id == seed.ClientId);

                if (client is null)
                {
                    _logger.LogWarning("Skipping case {CaseNumber}: missing client {ClientId}",
                        seed.CaseNumber, seed.ClientId);
                    continue;
                }

                // Create/Find client file for the case
                var currentHijriYear = HijriDateConverter.GetFullHijriYear();
                var clientFile = client.ClientFiles
                    .FirstOrDefault(cf => cf.CourtTypeCode == defaultCourtType.Code && cf.HijriYear == currentHijriYear);

                if (clientFile is null)
                {
                    var clientFileResult = ClientFile.Create(client.Id, defaultCourtType, (short)currentHijriYear);
                    if (clientFileResult.IsError)
                    {
                        _logger.LogWarning("Failed to create client file for client {ClientId}", client.Id);
                        continue;
                    }
                    clientFile = clientFileResult.Value;
                    //seededClientFiles.Add(clientFile);
                }

                // Link client to case
                var clientCase = new ClientCase(caseEntity.Id, client.Id, clientFile.Id);
                seededClientCases.Add(clientCase);
                     _context.Cases.Add(caseEntity);
                     _context.ClientFiles.Add(clientFile);
                     _context.ClientCases.Add(clientCase);
                await _context.SaveChangesAsync();


                _logger.LogInformation("Seeded case {CaseNumber} with client {ClientId}", seed.CaseNumber, client.Id);
            }

            //// Save all at once
            //if (seededCases.Any())
            //    await _context.Cases.AddRangeAsync(seededCases);

            //if (seededClientFiles.Any())
            //    await _context.ClientFiles.AddRangeAsync(seededClientFiles);

            //if (seededClientCases.Any())
            //    await _context.ClientCases.AddRangeAsync(seededClientCases);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeding of cases completed successfully.");
        }


    }
}

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initialiser.InitialiseAsync();
        await initialiser.EnsureViewsCreatedAsync(); // Create views before seeding
        await initialiser.SeedAsync();               // Seed data that might use views
    }
}