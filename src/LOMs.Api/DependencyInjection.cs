using LOMs.Api.Extensions;
using LOMs.Api.Infrastructure;
using LOMs.Api.Services;
using LOMs.Application.Common.Interfaces;
using LOMs.Contract.Commons.Enums;
using LOMs.Domain.Cases.Enums;
using LOMs.Domain.Cases.Judgments;
using LOMs.Domain.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all Presentation (API) layer services.
    /// </summary>
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        return services
            .AddIdentityServices()
            .AddExceptionHandling()
            .AddSwaggerConfiguration()
            .AddCorsConfiguration(configuration, env)
            .AddAppRateLimiting(configuration);
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
        services.AddHttpContextAccessor();
        return services;
    }

    private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }

    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "LOMs API", Version = "v1" });
            options.CustomSchemaIds(t => t.FullName);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token as: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });
        return services;
    }

    /// <summary>
    /// Registers JSON converters for MVC.
    /// </summary>
    public static IMvcBuilder ConfigureJsonConverters(this IMvcBuilder builder)
    {
        var converters = new JsonConverter[]
        {
            new PartyRoleJsonConverter(),
            new ContractTypeJsonConverter(),
            new CaseStatusJsonConverter(),
            new PartyRoleArabicJsonConverter(),
            new CaseStatusArabicJsonConverter(),
            new cContractTypeJsonConverter(),
            new JudgmentTypeJsonConverter(),
            new JudgmentSubTypeJsonConverter(),
            new cJudgmentTypeJsonConverter(),
            new cJudgmentSubTypeJsonConverter(),
            new cRoleJsonConverter(),
            new RoleJsonConverter()
        };

        builder.AddJsonOptions(opts =>
        {
            foreach (var converter in converters)
            {
                if (!opts.JsonSerializerOptions.Converters.Contains(converter))
                    opts.JsonSerializerOptions.Converters.Add(converter);
            }
        });

        return builder;
    }

    private static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        var section = configuration.GetSection("AppSettings");
        var policyName = section["CorsPolicyName"]
                         ?? (env.IsDevelopment() ? "DevCors" : "ProdCorsPolicy");

        var allowedOrigins = section
            .GetSection("AllowedOrigins")
            .Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("DevCors", b =>
                b.AllowAnyOrigin()
                 .AllowAnyHeader()
                 .AllowAnyMethod());

            options.AddPolicy(policyName, b =>
            {
                if (env.IsDevelopment())
                {
                    b.WithOrigins(allowedOrigins)
         .AllowAnyHeader()
         .AllowAnyMethod().AllowCredentials();
                }
                else
                {
                    b.WithOrigins(allowedOrigins)
                     .AllowAnyHeader()
                     .WithMethods(
                         HttpMethods.Get,
                         HttpMethods.Post,
                         HttpMethods.Put,
                         HttpMethods.Delete)
                     .AllowCredentials()
                     .SetPreflightMaxAge(TimeSpan.FromHours(1));
                }
            });
        });

        services.AddSingleton(new CorsPolicyName(policyName));
        return services;
    }

    /// <summary>
    /// Configures the middleware pipeline.
    /// </summary>
    public static IApplicationBuilder UseCoreMiddlewares(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();
        app.UseStaticFiles();

        var policyName = app.ApplicationServices
            .GetRequiredService<CorsPolicyName>().Name;
        app.UseCors(policyName);

        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}

/// <summary>
/// Holds the resolved CORS policy name for runtime.
/// </summary>
public record CorsPolicyName(string Name);
