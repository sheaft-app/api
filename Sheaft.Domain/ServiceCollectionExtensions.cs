using Microsoft.Extensions.DependencyInjection;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Domain;

public static class ServiceCollectionInitializers
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccount, CreateAccount>();
        services.AddScoped<IHandleProductCode, HandleProductCode>();
        services.AddScoped<IRetrieveDefaultCatalog, RetrieveDefaultCatalog>();

        return services;
    }
}