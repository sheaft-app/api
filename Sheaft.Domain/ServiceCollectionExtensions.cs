using Microsoft.Extensions.DependencyInjection;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.DocumentManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using PublishOrders = Sheaft.Domain.OrderManagement.PublishOrders;

namespace Sheaft.Domain;

public static class ServiceCollectionInitializers
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccount, CreateAccount>();
        
        services.AddScoped<IHandleProductCode, HandleProductCode>();
        services.AddScoped<IHandleReturnableCode, HandleReturnableCode>();
        
        services.AddScoped<IRetrieveDefaultCatalog, RetrieveDefaultCatalog>();
        
        services.AddScoped<ICreateOrderDraft, CreateOrderDraft>();
        services.AddScoped<IPublishOrders, PublishOrders>();
        services.AddScoped<IAcceptOrders, AcceptOrders>();
        services.AddScoped<IFulfillOrders, FulfillOrders>();
        services.AddScoped<IDeliverOrders, DeliverOrders>();
        services.AddScoped<IValidateOrderDeliveryDate, ValidateOrderDeliveryDate>();
        
        services.AddScoped<ICancelInvoices, CancelInvoices>();
        services.AddScoped<ICreateInvoices, CreateInvoices>();
        services.AddScoped<IPublishInvoices, PublishInvoices>();
        
        services.AddScoped<IDocumentProcessorFactory, DocumentProcessorFactory>();
        services.AddScoped<IDocumentProcessor, PreparationDocumentProcessor>();
        

        return services;
    }
}