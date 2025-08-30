
using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using LOMs.Application.Features.Customers.Commands.CreateCustomer;
using LOMs.Application.Features.Customers.Queries.GetCustomerById;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddLiteBusRigstertion();
       

        return services;
    }
    public static IServiceCollection AddLiteBusRigstertion(this IServiceCollection services)
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

        });

        return services;
    }

}