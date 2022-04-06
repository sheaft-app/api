using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class AcceptOrderCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Accepted()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);

        var acceptOrderCommand = new AcceptOrderCommand(order.Identifier, new DeliveryDate(DateTimeOffset.UtcNow.AddDays(4)));

        var result =
            await handler.Handle(
                acceptOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order.Identifier));

        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Accepted, order.Status);
        Assert.AreEqual(DeliveryStatus.Pending, delivery.Status);
        Assert.AreEqual(acceptOrderCommand.NewDeliveryDate.Value.Value, delivery.ScheduledAt.Value);
        Assert.AreEqual(acceptOrderCommand.CreatedAt, order.AcceptedOn);
    }
    
    [Test]
    public async Task Create_Delivery_With_Order_Lines()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);

        var acceptOrderCommand = new AcceptOrderCommand(order.Identifier, new DeliveryDate(DateTimeOffset.UtcNow.AddDays(4)));

        var result =
            await handler.Handle(
                acceptOrderCommand,
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Orders.Any(o => o.OrderIdentifier == order.Identifier));

        Assert.IsNotNull(delivery);
        Assert.AreEqual(OrderStatus.Accepted, order.Status);
        Assert.AreEqual(DeliveryStatus.Pending, delivery.Status);
        Assert.AreEqual(2, delivery.Lines.Count());
    }

    private (AppDbContext, AcceptOrderHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<AcceptOrderHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler = new AcceptOrderHandler(uow, new AcceptOrders(new OrderRepository(context), new DeliveryRepository(context)));

        return (context, handler);
    }

    private static Order InitOrder(AppDbContext context)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false);

        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("", "", "", ""), order.SupplierIdentifier, new List<Order> {order});

        context.Add(order);
        context.Add(delivery);

        context.SaveChanges();

        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618