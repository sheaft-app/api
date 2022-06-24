using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class FulfillOrdersCommandShould
{
    [Test]
    public async Task Set_Status_As_Fulfilled_And_FulfilledOn()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, 5, new List<string>())), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(OrderStatus.Fulfilled, order.Status);
        Assert.AreEqual(fulfillOrderCommand.CreatedAt, order.FulfilledOn);
    }
    
    [Test]
    public async Task Schedule_Related_Delivery()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, 5, new List<string>())), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var delivery = context.Deliveries.Single(d => d.Id == order.DeliveryId);
        Assert.IsNotNull(delivery);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery.Status);
    }

    [Test]
    public async Task ReSchedule_Related_Delivery()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, 5, new List<string>())),
            DateTimeOffset.UtcNow.AddDays(4));

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var delivery = context.Deliveries.Single(d => d.Id == order.DeliveryId);
        Assert.IsNotNull(delivery);
        Assert.AreEqual(DeliveryStatus.Scheduled, delivery.Status);
        Assert.AreEqual(fulfillOrderCommand.NewDeliveryDate.Value, delivery.ScheduledAt.Value);
    }
    
    [Test]
    public async Task Add_OrderLines_To_Related_Delivery_With_Updated_Quantities()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, 5, new List<string>())), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var delivery = context.Deliveries.Single(d => d.Id == order.DeliveryId);
        Assert.IsNotNull(delivery);
        Assert.IsTrue(delivery.Lines.All(l => l.Quantity.Value == 5));
    }
    
    [Test]
    public async Task Add_Batches_To_Related_Delivery_Lines()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var batch = context.Batches.First();
        
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, 5, new List<string>{batch.Id.Value})), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var delivery = context.Deliveries.Single(d => d.Id == order.DeliveryId);
        Assert.IsNotNull(delivery);
        Assert.AreEqual(order.Lines.Count(), delivery.Lines.Sum(l => l.Batches.Count()));
    }

    [Test]
    public async Task Fail_If_BatchIdentifier_Not_Found()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, 5, new List<string>{BatchId.New().Value})), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.NotFound, result.Error.Kind);
        Assert.AreEqual("delivery.lines.batches.not.found", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_All_New_Quantities_Are_Lower_Than_Zero()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var batch = context.Batches.First();
        
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(l.Identifier, -1, new List<string>{batch.Id.Value})), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("delivery.requires.lines", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Product_Not_Found()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);
        var batch = context.Batches.First();
        
        var fulfillOrderCommand = new FulfillOrdersCommand(order.Id,
            order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLineQuantityDto(ProductId.New().Value, -1, new List<string>{batch.Id.Value})), null);

        var result = await handler.Handle(fulfillOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.NotFound, result.Error.Kind);
        Assert.AreEqual("delivery.lines.products.not.found", result.Error.Code);
    }

    private (AppDbContext, FulfillOrdersHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<FulfillOrdersHandler>();

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var customer1Account  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customer1Account);
        
        var customer2Account  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customer2Account);

        var agreements = new Dictionary<AccountId, DeliveryDay>
            {{customer1Account.Id, new DeliveryDay(DayOfWeek.Friday)}, {customer2Account.Id, new DeliveryDay(DayOfWeek.Monday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer1Account.Id, customer2Account.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierAccount.Id, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierAccount.Id, supplierProducts}});

        var handler = new FulfillOrdersHandler(uow,
            new FulfillOrders(new OrderRepository(context), new DeliveryRepository(context),
                new GenerateDeliveryCode(context), new CreateDeliveryLines(context)));

        return (context, handler);
    }

    private static Order InitOrder(AppDbContext context, DateTimeOffset? deliveryDate = null)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        context.Add(new Batch(new BatchNumber("0001"), BatchDateKind.DLC, DateTime.Now, DateTime.Now, supplier.Id));

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());

        order.Accept();

        var delivery = new Delivery(new DeliveryDate(deliveryDate ?? DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("test", new EmailAddress("ese@ese.com"), "street", "", "70000", "Test"), order.SupplierId, order.CustomerId, new List<Order> {order});

        context.Add(order);
        context.Add(delivery);

        context.SaveChanges();

        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618