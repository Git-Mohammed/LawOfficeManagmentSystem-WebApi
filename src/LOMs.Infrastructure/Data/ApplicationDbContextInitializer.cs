
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LOMs.Infrastructure.Data;

public class ApplicationDbContextInitializer(
    ILogger<ApplicationDbContextInitializer> logger,
    AppDbContext context)
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger = logger;
    private readonly AppDbContext _context = context;
    //private readonly UserManager<AppUser> _userManager = userManager;
    //private readonly RoleManager<IdentityRole> _roleManager = roleManager;

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

    //public async Task SeedAsync()
    //{
    //    try
    //    {
    //        await TrySeedAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while seeding the database.");
    //        throw;
    //    }
    //}

    //public async Task TrySeedAsync()
    //{
    //    // Default roles
    //    var managerRole = new IdentityRole(nameof(Role.Manager));

    //    if (_roleManager.Roles.All(r => r.Name != managerRole.Name))
    //    {
    //        await _roleManager.CreateAsync(managerRole);
    //    }

    //    var laborRole = new IdentityRole(nameof(Role.Labor));

    //    if (_roleManager.Roles.All(r => r.Name != laborRole.Name))
    //    {
    //        await _roleManager.CreateAsync(laborRole);
    //    }

    //    // Default users
    //    var manager = new AppUser
    //    {
    //        Id = "19a59129-6c20-417a-834d-11a208d32d96",
    //        Email = "pm@localhost",
    //        UserName = "pm@localhost",
    //        EmailConfirmed = true
    //    };

    //    if (_userManager.Users.All(u => u.Email != manager.Email))
    //    {
    //        await _userManager.CreateAsync(manager, manager.Email);

    //        if (!string.IsNullOrWhiteSpace(managerRole.Name))
    //        {
    //            await _userManager.AddToRolesAsync(manager, [managerRole.Name]);
    //        }
    //    }

    //    var labor01 = new AppUser
    //    {
    //        Id = "b6327240-0aea-46fc-863a-777fc4e42560",
    //        Email = "john.labor@localhost",
    //        UserName = "john.labor@localhost",
    //        EmailConfirmed = true
    //    };

    //    if (_userManager.Users.All(u => u.Email != labor01.Email))
    //    {
    //        await _userManager.CreateAsync(labor01, labor01.Email);

    //        if (!string.IsNullOrWhiteSpace(laborRole.Name))
    //        {
    //            await _userManager.AddToRolesAsync(labor01, [laborRole.Name]);
    //        }
    //    }

    //    var labor02 = new AppUser
    //    {
    //        Id = "8104ab20-26c2-4651-b1de-c0baf04dbbd9",
    //        Email = "peter.labor@localhost",
    //        UserName = "peter.labor@localhost",
    //        EmailConfirmed = true
    //    };

    //    if (_userManager.Users.All(u => u.Email != labor02.Email))
    //    {
    //        await _userManager.CreateAsync(labor02, labor02.Email);

    //        if (!string.IsNullOrWhiteSpace(laborRole.Name))
    //        {
    //            await _userManager.AddToRolesAsync(labor02, [laborRole.Name]);
    //        }
    //    }

    //    var labor03 = new AppUser
    //    {
    //        Id = "e17c83de-1089-4f19-bf79-5f789133d37f",
    //        Email = "kevin.labor@localhost",
    //        UserName = "kevin.labor@localhost",
    //        EmailConfirmed = true
    //    };

    //    if (_userManager.Users.All(u => u.Email != labor03.Email))
    //    {
    //        await _userManager.CreateAsync(labor03, labor03.Email);

    //        if (!string.IsNullOrWhiteSpace(laborRole.Name))
    //        {
    //            await _userManager.AddToRolesAsync(labor03, [laborRole.Name]);
    //        }
    //    }

    //    var labor04 = new AppUser
    //    {
    //        Id = "54cd01ba-b9ae-4c14-bab6-f3df0219ba4c",
    //        Email = "suzan.labor@localhost",
    //        UserName = "suzan.labor@localhost",
    //        EmailConfirmed = true
    //    };

    //    if (_userManager.Users.All(u => u.Email != labor04.Email))
    //    {
    //        await _userManager.CreateAsync(labor04, labor04.Email);

    //        if (!string.IsNullOrWhiteSpace(laborRole.Name))
    //        {
    //            await _userManager.AddToRolesAsync(labor04, [laborRole.Name]);
    //        }
    //    }

    //    if (!_context.Employees.Any())
    //    {
    //        _context.Employees.AddRange(
    //        [
    //            Employee.Create(Guid.Parse(manager.Id), "Primary", "Manager", Role.Manager).Value,
    //            Employee.Create(Guid.Parse(labor01.Id), "John", "S.", Role.Labor).Value,
    //            Employee.Create(Guid.Parse(labor02.Id), "Peter", "R.", Role.Labor).Value,
    //            Employee.Create(Guid.Parse(labor03.Id), "Kevin", "M.", Role.Labor).Value,
    //            Employee.Create(Guid.Parse(labor04.Id), "Suzan", "L.", Role.Labor).Value
    //        ]);
    //    }

    //    if (!_context.Customers.Any())
    //    {
    //        List<Vehicle> vehicles = [
    //                    Vehicle.Create(id: Guid.Parse("61401e63-007b-4b1c-8914-9eb6e9bd95c5"), make: "Toyota", model: "Camry", year: 2020, licensePlate: "ABC123").Value,
    //                    Vehicle.Create(id: Guid.Parse("13c80914-41ad-4d46-b7bb-60f6c89ad01e"), make: "Honda", model: "Civic", year: 2018, licensePlate: "XYZ456").Value,
    //                ];

    //        _context.Customers.AddRange(
    //        [
    //            Customer.Create(id: Guid.Parse("f522bbe5-e3b1-4e2c-a8a3-c41550dcf39d"), name: "John Doe", phoneNumber: "123456789", email: "john.doe@localhost", vehicles: vehicles).Value,
    //            Customer.Create(id: Guid.Parse("73a04dd3-c81a-4a54-9882-ef1017eb192d"), name: "Sarah Peter", phoneNumber: "987654321", email: "sarah.peter@localhost", vehicles: [Vehicle.Create(id: Guid.Parse("a04f329d-0f5a-46a0-beae-699c034ae401"), make: "Ford", model: "Focus", year: 2021, licensePlate: "DEF789").Value, Vehicle.Create(id: Guid.Parse("cf60e95b-5752-4c26-aa07-31a34164606c"), make: "Chevrolet", model: "Malibu", year: 2019, licensePlate: "GHI012").Value,]).Value,
    //        ]);
    //    }
    //}
}

//public static class InitialiserExtensions
//{
//    public static async Task InitialiseDatabaseAsync(this WebApplication app)
//    {
//        using var scope = app.Services.CreateScope();

//        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

//        await initialiser.InitialiseAsync();

//        await initialiser.SeedAsync();
//    }
//}