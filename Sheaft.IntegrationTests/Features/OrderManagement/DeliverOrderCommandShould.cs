using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class DeliverOrderCommandShould
{
    [Test]
    public async Task Set_Order_Status_To_Completed_And_CompletedOn()
    {
        var (context, handler) = InitHandler();
        var delivery = InitDelivery(context);

        var deliverOrderCommand = new DeliverOrderCommand(delivery.Identifier, null, null);
        var result = await handler.Handle(deliverOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var order = context.Orders.Single(d => delivery.SupplierIdentifier == d.SupplierIdentifier);
        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Completed, order.Status);
        Assert.AreEqual(deliverOrderCommand.CreatedAt, order.CompletedOn);
    }
    
    [Test]
    public async Task Set_Delivery_Status_To_Delivered_And_DeliveredOn()
    {
        var (context, handler) = InitHandler();
        var delivery = InitDelivery(context);

        var deliverOrderCommand = new DeliverOrderCommand(delivery.Identifier, null, null);
        var result = await handler.Handle(deliverOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var order = context.Orders.Single(d => delivery.SupplierIdentifier == d.SupplierIdentifier);
        Assert.IsNotNull(order);
        Assert.AreEqual(DeliveryStatus.Delivered, delivery.Status);
        Assert.AreEqual(deliverOrderCommand.CreatedAt, delivery.DeliveredOn);
    }

    [Test]
    public async Task Add_Products_Adjustments_To_Delivery()
    {
        var (context, handler) = InitHandler();
        var delivery = InitDelivery(context);
        var products = context.Products.ToList();
        var deliverOrderCommand = new DeliverOrderCommand(delivery.Identifier,
            new List<ProductAdjustment>
            {
                new ProductAdjustment(products.First().Identifier, new Quantity(1)),
                new ProductAdjustment(products.Skip(1).First().Identifier, new Quantity(-1))
            }, null);
        
        var result = await handler.Handle(deliverOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(3, delivery.Adjustments.Count());
        var firstProduct = delivery.Adjustments.First(a => a.Identifier == products.First().Identifier.Value);
        Assert.IsNotNull(firstProduct);
        Assert.AreEqual(DeliveryLineKind.Product, firstProduct.LineKind);
        Assert.AreEqual(1, firstProduct.Quantity.Value);
        var firstReturnable = delivery.Adjustments.First(a => a.Identifier == products.First().Returnable.Identifier.Value);
        Assert.IsNotNull(firstReturnable);
        Assert.AreEqual(DeliveryLineKind.Returnable, firstReturnable.LineKind);
        Assert.AreEqual(1, firstReturnable.Quantity.Value);
        var secondProduct = delivery.Adjustments.First(a => a.Identifier == products.Skip(1).First().Identifier.Value);
        Assert.IsNotNull(secondProduct);
        Assert.AreEqual(DeliveryLineKind.Product, secondProduct.LineKind);
        Assert.AreEqual(-1, secondProduct.Quantity.Value);
    }

    [Test]
    public async Task Add_Returned_Returnables_To_Delivery()
    {
        var (context, handler) = InitHandler();
        var delivery = InitDelivery(context);
        var returnable = context.Returnables.First();
        var deliverOrderCommand = new DeliverOrderCommand(delivery.Identifier, null,
            new List<ReturnedReturnable>
            {
                new ReturnedReturnable(returnable.Identifier, new Quantity(-1))
            });
        
        var result = await handler.Handle(deliverOrderCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, delivery.Adjustments.Count());
        var deliveryLine = delivery.Adjustments.First();
        Assert.AreEqual(DeliveryLineKind.ReturnedReturnable, deliveryLine.LineKind);
        Assert.AreEqual(returnable.Identifier.Value, deliveryLine.Identifier);
        Assert.AreEqual(-1, deliveryLine.Quantity.Value);
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

        var supplier1 = context.Suppliers.First();
        context.Returnables.Add(new Returnable(new ReturnableName("test"), new ReturnableReference("re"),
            new UnitPrice(2000), new VatRate(2000), supplier1.Identifier));

        context.SaveChanges();
            
        var handler = new DeliverOrderHandler(uow,
            new DeliverOrders(
                new OrderRepository(context),
                new DeliveryRepository(context),
                new CreateDeliveryProductAdjustments(context),
                new CreateDeliveryReturnedReturnables(context)));

        return (context, handler);
    }

    private static Delivery InitDelivery(AppDbContext context)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());

        order.Accept();
        order.Fulfill();

        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("street", "", "70000", "Test"), order.SupplierIdentifier, new List<Order> {order});

        delivery.Schedule(new DeliveryReference(Guid.NewGuid().ToString("N")),
            new DeliveryDate(DateTimeOffset.UtcNow.AddDays(4)), order.Lines.Where(l => l.LineKind == OrderLineKind.Product)
                .Select(l => new DeliveryLine(l.Identifier, DeliveryLineKind.Product, l.Reference, l.Name, l.Quantity, l.UnitPrice, l.Vat)), new List<DeliveryBatch>(), DateTimeOffset.UtcNow);

        context.Add(order);
        context.Add(delivery);

        var returnable = new Returnable(new ReturnableName("er"), new ReturnableReference("code"), new UnitPrice(2000),
            new VatRate(2000), supplier.Identifier);
        context.Add(returnable);

        var product = context.Products.First();
        product.SetReturnable(returnable);
        
        context.SaveChanges();

        return delivery;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618