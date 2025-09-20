
using LOMs.Domain.Cases.CourtTypes;
using  LOMs.Domain.Identity;
using LOMs.Domain.People;
using LOMs.Domain.People.Clients;
using LOMs.Domain.People.Employees;
using LOMs.Infrastructure.Data;
using LOMs.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
            Phone = "+966504567890",
            Address = "Abha, Saudi Arabia",
            Email = "arammtech@gmail.com",
            Role = Role.GeneralManager.ToString(),
            Password = "Admin123@"
        },
        new
        {
            PersonId = Guid.Parse("44444444-dddd-4eee-ffff-444444444444"),
            Name = "Salem Al-Dosari",
            NationalId = "4567890123",
            Nationality = "SA",
            BirthDate = new DateOnly(1988, 7, 15),
            Phone = "+966504567890",
            Address = "Abha, Saudi Arabia",
            Email = "salem.dosari@example.com",
            Role = Role.Lawyer.ToString(),
            Password = "P@ssword123"
        },
        new
        {
            PersonId = Guid.Parse("55555555-eeee-4fff-aaaa-555555555555"),
            Name = "Noura Al-Fahad",
            NationalId = "5678901234",
            Nationality = "SA",
            BirthDate = new DateOnly(1991, 3, 22),
            Phone = "+966505678901",
            Address = "Tabuk, Saudi Arabia",
            Email = "noura.fahad@example.com",
            Role = Role.Consultant.ToString(),
            Password = "P@ssword123"
        }
    };

            foreach (var data in employeesData)
            {
                var person = Person.Create(data.PersonId, data.Name, data.NationalId, data.Nationality, data.BirthDate, data.Phone, data.Address).Value;
                var employee = Employee.Create(Guid.NewGuid(), data.Email, person, data.Role).Value;

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

                await userManager.AddToRoleAsync(identityUser, data.Role);
                employee.AssignUser(identityUser.Id);

                _context.People.Add(person);
                _context.Employees.Add(employee);
            }

            await _context.SaveChangesAsync();
        }


        // --- Clients Seed Data ---
        if (!_context.Clients.Any())
        {
            var clients = new List<Client>();

            // Client 1
            var person1 = Person.Create(
                Guid.Parse("11111111-aaaa-4bbb-cccc-111111111111"),
                "Ahmed Al-Harbi",
                "1234567890",
                "SA",
                new DateOnly(1990, 5, 12),
                "+966501234567",
                "Riyadh, Saudi Arabia"
            ).Value;

            var client1 = Client.Create(
                Guid.Parse("aaaa1111-bbbb-4ccc-dddd-111111111111"),
                person1
            ).Value;

            // Client 2
            var person2 = Person.Create(
                Guid.Parse("22222222-bbbb-4ccc-dddd-222222222222"),
                "Fatimah Al-Qahtani",
                "2345678901",
                "SA",
                new DateOnly(1985, 11, 3),
                "+966502345678",
                "Jeddah, Saudi Arabia"
            ).Value;

            var client2 = Client.Create(
                Guid.Parse("bbbb2222-cccc-4ddd-eeee-222222222222"),
                person2
            ).Value;

            // Client 3
            var person3 = Person.Create(
                Guid.Parse("33333333-cccc-4ddd-eeee-333333333333"),
                "Mohammed Al-Shehri",
                "3456789012",
                "SA",
                new DateOnly(1992, 2, 20),
                "+966503456789",
                "Dammam, Saudi Arabia"
            ).Value;

            var client3 = Client.Create(
                Guid.Parse("cccc3333-dddd-4eee-ffff-333333333333"),
                person3
            ).Value;

            // Add to context
            _context.People.AddRange(person1, person2, person3);
            _context.Clients.AddRange(client1, client2, client3);

            await _context.SaveChangesAsync();
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

        await initialiser.SeedAsync();
    }
}