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
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class FulfillOrderCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Fulfilled()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);

        var fulfillOrderCommand = new FulfillOrderCommand(order.Identifier, new DeliveryDate(DateTimeOffset.UtcNow.AddDays(4)));
        
        var result =
            await handler.Handle(
                fulfillOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order.Identifier));

        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Fulfilled, order.Status);
        Assert.AreEqual(fulfillOrderCommand.NewDeliveryDate.Value.Value, delivery.ScheduledAt.Value);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order.FulfilledOn);
    }

    private (AppDbContext, FulfillOrderHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<FulfillOrderHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler = new FulfillOrderHandler(uow, new FulfillOrders(new OrderRepository(context), new DeliveryRepository(context), new GenerateDeliveryCode()));

        return (context, handler);
    }

    private static Order InitOrder(AppDbContext context)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false);

        order.Accept();

        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("", "", "", ""), new List<OrderId> {order.Identifier}, order.SupplierIdentifier);

        context.Add(order);
        context.Add(delivery);
        
        context.SaveChanges();
        
        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618