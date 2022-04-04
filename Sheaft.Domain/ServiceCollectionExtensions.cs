using Microsoft.Extensions.DependencyInjection;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Domain;

public static class ServiceCollectionInitializers
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccount, CreateAccount>();
        
        services.AddScoped<IHandleProductCode, HandleProductCode>();
        
        services.AddScoped<IRetrieveDefaultCatalog, RetrieveDefaultCatalog>();
        
        services.AddScoped<ICreateOrderDraft, CreateOrderDraft>();
        services.AddScoped<IPublishOrders, PublishOrders>();
        services.AddScoped<IAcceptOrders, AcceptOrders>();
        services.AddScoped<IFulfillOrders, FulfillOrders>();
        services.AddScoped<IDeliverOrders, DeliverOrders>();
        services.AddScoped<IValidateOrderDeliveryDate, ValidateOrderDeliveryDate>();

        return services;
    }
}