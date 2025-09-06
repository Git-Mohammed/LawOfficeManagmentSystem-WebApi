using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Events.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using LOMs.Application.Features.Cases.Commands.CreateCase;
using LOMs.Application.Features.Cases.Queries.GetCaseByIdQuery;
using LOMs.Application.Features.ClientFiles;
using LOMs.Application.Features.Contracts.Services;
using LOMs.Application.Features.CourtTypes;
using LOMs.Application.Features.People.Employees.EventHandlers;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddLiteBusRegistration()
            .RegisterFactories()
            .RegisterServices();

        return services;
    }

    private static IServiceCollection AddLiteBusRegistration(this IServiceCollection services)
    {
        services.AddLiteBus(liteBus =>
        {
            liteBus.AddCommandModule(module =>
            {
                module.RegisterFromAssembly(typeof(CreateCaseCommand).Assembly);
            });

            liteBus.AddQueryModule(module =>
            {
                module.RegisterFromAssembly(typeof(GetCaseByIdQuery).Assembly);
            });
            liteBus.AddEventModule(module =>
            {
                module.RegisterFromAssembly(typeof(SendTemporaryPasswordEmailHandler).Assembly);
            });
        });

        return services;
    }

    private static IServiceCollection RegisterFactories(this IServiceCollection services)
    {
        services.AddScoped<IClientFileFactory, ClientFileFactory>();
        services.AddScoped<IContractFactory, ContractFactory>();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICourtTypeService, CourtTypeService>();

        return services;
    }
}