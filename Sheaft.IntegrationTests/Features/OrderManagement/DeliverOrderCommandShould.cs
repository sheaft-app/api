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

public class DeliverOrderCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Delivered()
    {
        var (context, handler) = InitHandler();
        var delivery = InitOrder(context);

        var deliverOrderCommand = new DeliverOrderCommand(delivery.Identifier, null, null);
        var result =
            await handler.Handle(
                deliverOrderCommand,
                CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);

        var order = context.Orders.Single(d => delivery.SupplierIdentifier == d.SupplierIdentifier);

        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Completed, order.Status);
        Assert.AreEqual(DeliveryStatus.Delivered, delivery.Status);
        Assert.AreEqual(deliverOrderCommand.CreatedAt, delivery.DeliveredOn);
    }

    private (AppDbContext, DeliverOrderHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<DeliverOrderHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler = new DeliverOrderHandler(uow, new DeliverOrders(new OrderRepository(context), new DeliveryRepository(context)));

        return (context, handler);
    }

    private static Delivery InitOrder(AppDbContext context)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false);

        order.Accept();
        order.Fulfill();
        
        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("street", "", "70000", "Test"), order.SupplierIdentifier, new List<Order> {order});

        delivery.Schedule(new DeliveryCode(Guid.NewGuid().ToString("N")), new DeliveryDate(DateTimeOffset.UtcNow.AddDays(4)),
            DateTimeOffset.UtcNow);
        
        context.Add(order);
        context.Add(delivery);
        
        context.SaveChanges();
        
        return delivery;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618