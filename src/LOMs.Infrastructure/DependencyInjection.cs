using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Options;
using LOMs.Infrastructure.Data;
using LOMs.Infrastructure.Data.Interceptors;
using LOMs.Infrastructure.Identity;
using LOMs.Infrastructure.Mapping.Configs;
using LOMs.Infrastructure.Services;
using LOMs.Infrastructure.Services.DomainEventPublishers;
using LOMs.Infrastructure.Services.EmailSender;
using LOMs.Infrastructure.Services.FileServices;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);            

        RegisterDatabase(services, configuration);
        RegisterIdentity(services);
        RegisterAuthentication(services, configuration);
        RegisterApplicationServices(services, configuration);
        RegisterOptions(services, configuration);

        return services;
    }

    // 🔹 Database & EF Core
    private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();
    }

    // 🔹 Identity & Security
    private static void RegisterIdentity(IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 1;
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();
    }

    // 🔹 JWT Authentication
    private static void RegisterAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
            };
        });
    }

    // 🔹 Application Services
    private static void RegisterApplicationServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDomainEventPublisher, LiteBusEventPublisher>();
        services.AddPasswordGenerator();
        services.AddEmailSenderService(configuration);
        services.AddFileService();
        services.MapsterRegister();
    }

    // 🔹 Configuration Binding
    private static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<PasswordGeneratorOptions>(configuration.GetSection("PasswordGenerator"));
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
    }

    // 🔹 Mapster Mapping
    private static IServiceCollection MapsterRegister(this IServiceCollection services)
    {
        MappingConfig.Configure();
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<MapsterMapper.IMapper, MapsterMapper.ServiceMapper>();
        services.AddScoped<IMapper, MappingServiceAdapter>();
        return services;
    }

    // 🔹 Password Generator
    private static IServiceCollection AddPasswordGenerator(this IServiceCollection services)
    {
        return services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
    }

    // 🔹 Email Sender
    private static IServiceCollection AddEmailSenderService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        return services;
    }

    // 🔹 File Service
    private static IServiceCollection AddFileService(this IServiceCollection services)
    {
        services.AddSingleton<IFileValidator, FileValidator>();
        services.AddScoped<IFileService>(sp =>
            new LocalFileService("uploads", sp.GetRequiredService<IFileValidator>()));
        return services;
    }
}
