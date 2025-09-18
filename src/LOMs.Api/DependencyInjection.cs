using System.Reflection;
using LOMs.Api.Infrastructure;
using LOMs.Api.Services;
using LOMs.Application.Common.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using static System.Net.WebRequestMethods;

namespace LOMs.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityInfrastructure()
            .AddExceptionHandlingInfrastructure()
            .AddSwaggerInfrastructure()
            .AddCorsInfrastructure(configuration);

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

    private static IServiceCollection AddSwaggerInfrastructure(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "LOMs API", Version = "v1" });
            options.CustomSchemaIds(type => type.FullName);

            //  JWT Bearer token support
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token in the format: Bearer {your token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            //  XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (System.IO.File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    private static IServiceCollection AddCorsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var policyName = configuration["AppSettings:CorsPolicyName"] ?? "AllowAll";

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.WithOrigins(["https://localhost:4200"])
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
                       
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
    {
        // 1. Exception handling should be FIRST to catch all errors
        app.UseExceptionHandler();

        // 2. HTTPS redirection
        app.UseHttpsRedirection();

        // 3. Static files
        app.UseStaticFiles();

        // 4. CORS
        var policyName = configuration["AppSettings:CorsPolicyName"] ?? "AllowAll";
        app.UseCors(policyName);

        // 5. Authentication
        app.UseAuthentication();

        // 6. Authorization
        app.UseAuthorization();

        return app;
    }
}
