using LOMs.Api.Services;
using LOMs.Application.Common.Interfaces;
using LOMs.Api.Infrastructure; // <-- Add this for GlobalExceptionHandler
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LOMs.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddIdentityInfrastructure();
        services.AddExceptionHandlingInfrastructure(); // <-- Add this line
        return services;
    }

    private static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
        services.AddHttpContextAccessor();
        return services;
    }

    private static IServiceCollection AddExceptionHandlingInfrastructure(this IServiceCollection services)
    {
        services.AddProblemDetails(); // Enables IProblemDetailsService
        services.AddExceptionHandler<GlobalExceptionHandler>(); // Registers your custom handler
        return services;
    }

    public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
    {
        // 1. Exception handling should be FIRST to catch all errors
        app.UseExceptionHandler();

        // 2. Status code pages for handling HTTP status codes
   //     app.UseStatusCodePages();

        // 3. HTTPS redirection (before any other middleware that might generate URLs)
        app.UseHttpsRedirection();

        // 5. CORS (before authentication/authorization)
      //  app.UseCors(configuration["AppSettings:CorsPolicyName"]!);

        // 6. Rate limiting (before authentication to protect auth endpoints)
    //    app.UseRateLimiter();

        // 7. Authentication (must come before authorization)
    //    app.UseAuthentication();

        // 8. Authorization (must come after authentication)
    //    app.UseAuthorization();

        // 9. Output caching (after auth to cache based on user context)
     //   app.UseOutputCache();

        return app;
    }
}
