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

public class UpdateOrderDraftProductsCommandShould
{
    [Test]
    public async Task Add_Lines_On_Order()
    {
        var (context, handler) = InitHandler();

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier);
        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Where(p => p.SupplierIdentifier == supplier.Identifier)
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .ToList();

        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Identifier, lines),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
    }

    [Test]
    public async Task Update_Lines_Quantity_On_Order()
    {
        var (context, handler) = InitHandler();

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var product = context.Products.First();
        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier);
        order.UpdateDraftLines(new List<OrderLine>
        {
            OrderLine.CreateProductLine(product.Identifier, product.Reference, product.Name, new OrderedQuantity(1),
                new ProductUnitPrice(1000), new VatRate(1000))
        });

        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Where(p => p.SupplierIdentifier == supplier.Identifier)
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .ToList();

        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Identifier, lines),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
        Assert.AreEqual(2, order.Lines.First(l => l.Identifier == product.Identifier.Value).PriceInfo.Quantity.Value);
    }

    [Test]
    public async Task Update_Only_With_Products_From_Order_Supplier()
    {
        var (context, handler) = InitHandler();

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var product = context.Products.First(o => o.SupplierIdentifier == supplier.Identifier);
        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier);
        order.UpdateDraftLines(new List<OrderLine>
        {
            OrderLine.CreateProductLine(product.Identifier, product.Reference, product.Name, new OrderedQuantity(1),
                new ProductUnitPrice(1000), new VatRate(1000))
        });

        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .ToList();

        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Identifier, lines),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
        Assert.AreEqual(2, order.Lines.First(l => l.Identifier == product.Identifier.Value).PriceInfo.Quantity.Value);
    }

    [Test]
    public async Task Remove_Lines_On_Order()
    {
        var (context, handler) = InitHandler();

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, true);

        context.Add(order);
        context.SaveChanges();

        var line = context.Products
            .Where(p => p.SupplierIdentifier == supplier.Identifier)
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .FirstOrDefault();

        var result =
            await handler.Handle(
                new UpdateOrderDraftProductsCommand(order.Identifier, new List<ProductQuantityDto> {line}),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, order.Lines.Count());
    }

    private (AppDbContext, UpdateOrderDraftProductsHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<UpdateOrderDraftProductsHandler>();

        var supplier = AccountId.New();
        var supplier1 = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};
        var supplier1Products = new Dictionary<string, int> {{"T1", 1000}, {"T2", 5000}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>>
                {{supplier, agreements}, {supplier1, new Dictionary<AccountId, DeliveryDay>()}},
            new Dictionary<AccountId, Dictionary<string, int>>
                {{supplier, supplierProducts}, {supplier1, supplier1Products}});

        var handler = new UpdateOrderDraftProductsHandler(uow, new TransformProductsToOrderLines(context));

        return (context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618