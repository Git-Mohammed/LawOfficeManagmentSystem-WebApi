using LOMs.Infrastructure.Data.Views.Registry;
using LOMs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using LOMs.Infrastructure.Data.Seeds;
using LOMs.Infrastructure.Constants;

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
        // Roles
        foreach (var roleName in SeedData.Roles.Names)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                _logger.LogInformation("Created role {Role}.", roleName);
            }
        }

        // Default admin user
        var admin = await _userManager.FindByNameAsync(SeedData.DefaultUser.NationalId);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName       = SeedData.DefaultUser.NationalId,
                Email          = SeedData.DefaultUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin, SeedData.DefaultUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, SeedData.DefaultUser.TheRole.ToString());
                _logger.LogInformation("Created default user {User}.", admin.UserName);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed creating default user: {Errors}", errors);
            }
        }


        // You can add more lookup or client seed sections here...
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
