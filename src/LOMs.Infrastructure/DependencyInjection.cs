using LOMs.Application.Common.Interfaces;
using LOMs.Infrastructure.Data;
using LOMs.Infrastructure.Data.Interceptors;
using LOMs.Infrastructure.Identity;
using LOMs.Infrastructure.Mapping.Configs;
using LOMs.Infrastructure.Services;
using LOMs.Infrastructure.Services.DomainEventPublishers;
using LOMs.Infrastructure.Services.EmailSender;
using LOMs.Infrastructure.Services.FileServices;
using LOMs.Infrastructure.Services.Image;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;



namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.SignIn.RequireConfirmedAccount = false;
            }).AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IIdentityService, IdentityService>();
        services.MapsterRegister();
        services.AddPasswordGenerator();
        services.AddEmailSenderService(configuration);
        services.AddScoped<IDomainEventPublisher,LiteBusEventPublisher>();
        services.AddServices();
        services.AddFileService();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
    private static IServiceCollection MapsterRegister(this IServiceCollection services)
    {
        MappingConfig.Configure();
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<MapsterMapper.IMapper, MapsterMapper.ServiceMapper>();
        services.AddScoped<IMapper, MappingServiceAdapter>();
        return services;
    }
    
    private static IServiceCollection AddPasswordGenerator(this IServiceCollection services)
    {
        return services.AddSingleton<IPasswordGenerator, RandomPasswordGenerator>();
    }
    private static IServiceCollection AddEmailSenderService(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind the "Smtp" section to SmtpSettings
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));

        // Register the email sender
        services.AddScoped<IEmailSender, SmtpEmailSender>();

        return services;
    }

    private static IServiceCollection AddFileService(this IServiceCollection services)
    {
        services.AddSingleton<IFileValidator,FileValidator>();
        services.AddScoped<IFileService>(sp =>
            new LocalFileService("uploads",sp.GetRequiredService<IFileValidator>()));
        return services;
    }
}
