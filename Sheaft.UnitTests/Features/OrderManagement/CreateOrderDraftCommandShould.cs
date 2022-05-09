using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.OrderManagement;

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
    public async Task Not_Create_Delivery_Related_To_Order()
    {
        var (context, handler) = InitHandler();
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();
        
        var result = await handler.Handle(new CreateOrderDraftCommand(supplier.Identifier, customer.Identifier), CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        var order = context.Orders.Single(c => c.Identifier == new OrderId(result.Value));
        var delivery = context.Deliveries.SingleOrDefault(c => c.Identifier == order.DeliveryIdentifier);
        Assert.IsNull(delivery);
    }
    
    [Test]
    public async Task Return_Existing_Order_Draft_If_A_Draft_Already_Exists()
    {
        var (context, handler) = InitHandler();
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();
        
        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier);
        
        context.Orders.Add(order);
        context.SaveChanges();
        
        var result = await handler.Handle(new CreateOrderDraftCommand(supplier.Identifier, customer.Identifier), CancellationToken.None);
    
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(order.Identifier.Value, result.Value);
    }
    
    [Test]
    public async Task Fail_If_No_Agreement_Exists()
    {
        var (context, handler) = InitHandler(false);
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();
        
        var result = await handler.Handle(new CreateOrderDraftCommand(supplier.Identifier, customer.Identifier), CancellationToken.None);
    
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.requires.agreement", result.Error.Code);
    }

    private (AppDbContext, CreateOrderDraftHandler) InitHandler(bool addAgreement = true)
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<CreateOrderDraftHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay>();
        if (addAgreement)
            agreements.Add(customer, new DeliveryDay(DayOfWeek.Friday));
        
        DataHelpers.InitContext(context,
            new List<AccountId>{customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>>
                {{supplier, new Dictionary<string, int> {{"001", 2000}, {"002", 3500}}}});
        
        var handler = new CreateOrderDraftHandler(uow, new CreateOrderDraft(new OrderRepository(context), new RetrieveAgreementForOrder(context)));
        
        return (context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
