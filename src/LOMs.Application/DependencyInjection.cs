
using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Events.Abstractions;
using LiteBus.Events.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Customers.Commands.CreateCustomer;
using LOMs.Application.Features.Customers.Queries.GetCustomerById;
using LOMs.Application.Features.People.Employees.EventHandlers;
using LOMs.Domain.People.Employees.DomainEvents;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddLiteBusRigstertion();
       

        return services;
    }


    private static IServiceCollection AddLiteBusRigstertion(this IServiceCollection services)
    {
        services.AddLiteBus(liteBus =>
        {
            liteBus.AddCommandModule(module =>
            {
                module.RegisterFromAssembly(typeof(CreateCustomerCommand).Assembly);
            });

            liteBus.AddQueryModule(module =>
            {
                module.RegisterFromAssembly(typeof(GetCustomerByIdQuery).Assembly);
            });
            liteBus.AddEventModule(module =>
            {
                module.RegisterFromAssembly(typeof(SendTemporaryPasswordEmailHandler).Assembly);
            });

        });
        //services.AddTransient<IEventHandler<EmployeeCreatedEvent>, SendTemporaryPasswordEmailHandler>();
        return services;
    }
    
}