using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CreateOrderDraftCommandShould
{
    [Test]
    public async Task Create_Draft()
    {
        var (context, handler) = InitHandler();
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();
        
        var result = await handler.Handle(new CreateOrderDraftCommand(supplier.Identifier, customer.Identifier), CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);
        
        var order = context.Orders.Single(c => c.Identifier == new OrderId(result.Value));
        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Draft, order.Status);
    }
    
    [Test]
    public async Task Return_Existing_Order_Draft()
    {
        var (context, handler) = InitHandler();
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier, customer.DeliveryAddress,
            new BillingAddress(customer.Legal.Address.Street, customer.Legal.Address.Complement,
                customer.Legal.Address.Postcode, customer.Legal.Address.City));
        
        context.Orders.Add(order);
        context.SaveChanges();
        
        var result = await handler.Handle(new CreateOrderDraftCommand(supplier.Identifier, customer.Identifier), CancellationToken.None);
    
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(order.Identifier.Value, result.Value);
    }

    private (AppDbContext, CreateOrderDraftHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<CreateOrderDraftHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();
        
        DataHelpers.InitContext(context,
            new List<AccountId>{customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, new Dictionary<AccountId, DeliveryDay>{{customer, new DeliveryDay(DayOfWeek.Friday)}}}},
            new Dictionary<AccountId, Dictionary<string, int>>
                {{supplier, new Dictionary<string, int> {{"001", 2000}, {"002", 3500}}}});
        
        var handler = new CreateOrderDraftHandler(uow);
        
        return (context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
