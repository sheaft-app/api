using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.OData.Edm;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class FulfillOrdersCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Fulfilled()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context, false);

        var fulfillOrderCommand = new FulfillOrdersCommand(new List<OrderId> {order.Identifier}, new List<CustomerDeliveryDate>());

        var result =
            await handler.Handle(
                fulfillOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order.Identifier));

        Assert.IsNotNull(delivery);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery.Status);
        Assert.AreEqual(OrderStatus.Fulfilled, order.Status);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order.FulfilledOn);
    }
    
    [Test]
    public async Task Switch_Order_Status_To_Fulfilled_And_Reschedule_Delivery()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context, false);

        var fulfillOrderCommand = new FulfillOrdersCommand(new List<OrderId> {order.Identifier},
            new List<CustomerDeliveryDate>
            {
                new CustomerDeliveryDate(order.CustomerIdentifier, new DeliveryDate(DateTimeOffset.UtcNow.AddDays(4)))
            });

        var result =
            await handler.Handle(
                fulfillOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order.Identifier));

        Assert.IsNotNull(delivery);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery.Status);
        Assert.AreEqual(OrderStatus.Fulfilled, order.Status);
        Assert.AreEqual(fulfillOrderCommand.CustomersNewDeliveryDate.First().DeliveryDate.Value,
            delivery.ScheduledAt.Value);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order.FulfilledOn);
    }

    [Test]
    public async Task Regroup_Orders_Into_Same_Delivery_If_Customer_Are_Identical()
    {
        var (context, handler) = InitHandler();
        var deliveryDate = DateTimeOffset.UtcNow.AddDays(2);
        var order1 = InitOrder(context, false, deliveryDate);
        var order2 = InitOrder(context, false, deliveryDate);

        var fulfillOrderCommand = new FulfillOrdersCommand(new List<OrderId> {order1.Identifier, order2.Identifier},
            new List<CustomerDeliveryDate>());

        var result =
            await handler.Handle(
                fulfillOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single();

        Assert.IsNotNull(delivery);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery.Status);
        Assert.AreEqual(2, delivery.Orders.Count());
        Assert.AreEqual(OrderStatus.Fulfilled, order1.Status);
        Assert.AreEqual(OrderStatus.Fulfilled, order2.Status);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order1.FulfilledOn);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order2.FulfilledOn);
        Assert.AreEqual(2, delivery.Lines.Count());
        Assert.AreEqual(4, delivery.Lines.Sum(l => l.Quantity.Value));
    }

    [Test]
    public async Task Not_Regroup_Orders_Into_Same_Delivery_If_Customer_Are_Different()
    {
        var (context, handler) = InitHandler();
        var order1 = InitOrder(context, false);
        var order2 = InitOrder(context, true);

        var fulfillOrderCommand = new FulfillOrdersCommand(new List<OrderId> {order1.Identifier, order2.Identifier}, new List<CustomerDeliveryDate>());

        var result =
            await handler.Handle(
                fulfillOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery1 = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order1.Identifier));
        var delivery2 = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order2.Identifier));

        Assert.IsNotNull(delivery1);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery1.Status);
        Assert.AreEqual(1, delivery1.Orders.Count());
        Assert.AreEqual(2, delivery1.Lines.Count());
        Assert.AreEqual(2, delivery1.Lines.Sum(l => l.Quantity.Value));
        
        Assert.IsNotNull(delivery2);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery2.Status);
        Assert.AreEqual(1, delivery2.Orders.Count());
        Assert.AreEqual(2, delivery2.Lines.Count());
        Assert.AreEqual(2, delivery2.Lines.Sum(l => l.Quantity.Value));
        
        Assert.AreEqual(OrderStatus.Fulfilled, order1.Status);
        Assert.AreEqual(OrderStatus.Fulfilled, order2.Status);
        
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order1.FulfilledOn);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order2.FulfilledOn);
    }

    private (AppDbContext, FulfillOrdersHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<FulfillOrdersHandler>();

        var supplier = AccountId.New();
        var customer1 = AccountId.New();
        var customer2 = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer1, new DeliveryDay(DayOfWeek.Friday)}, {customer2, new DeliveryDay(DayOfWeek.Monday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer1, customer2},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler = new FulfillOrdersHandler(uow,
            new FulfillOrders(new OrderRepository(context), new DeliveryRepository(context),
                new GenerateDeliveryCode()));

        return (context, handler);
    }

    private static Order InitOrder(AppDbContext context, bool otherCustomer, DateTimeOffset? deliveryDate = null)
    {
        var supplier = context.Suppliers.First();
        var customer = !otherCustomer ? context.Customers.OrderBy(c => c.AccountIdentifier).First() : context.Customers.OrderBy(c => c.AccountIdentifier).Last();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false);

        order.Accept();

        var delivery = new Delivery(new DeliveryDate(deliveryDate ?? DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("street", "", "70000", "Test"), order.SupplierIdentifier, new List<Order> {order});

        context.Add(order);
        context.Add(delivery);

        context.SaveChanges();

        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618