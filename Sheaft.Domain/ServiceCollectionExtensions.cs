using Microsoft.Extensions.DependencyInjection;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Domain;

public static class ServiceCollectionInitializers
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccount, CreateAccount>();

        return services;
    }
}