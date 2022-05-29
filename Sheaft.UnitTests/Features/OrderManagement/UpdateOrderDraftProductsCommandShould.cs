using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class UpdateOrderDraftProductsCommandShould
{
    [Test]
    public async Task Add_Lines_On_Order()
    {
        var (accountId, context, handler) = InitHandler();

        var supplier = context.Suppliers.First(s => s.AccountId == accountId);
        var customer = context.Customers.First();

        var order = Order.CreateDraft(supplier.Id, customer.Id);
        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Where(p => p.SupplierId == supplier.Id)
            .Select(p => new ProductQuantityDto(p.Id.Value, 2))
            .ToList();

        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Id, lines),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
    }

    [Test]
    public async Task Update_Lines_Quantity_On_Order()
    {
        var (accountId, context, handler) = InitHandler();

        var supplier = context.Suppliers.First(s => s.AccountId == accountId);
        var customer = context.Customers.First();

        var product = context.Products.First(c => c.SupplierId == supplier.Id);
        var order = Order.CreateDraft(supplier.Id, customer.Id);
        order.UpdateDraftLines(new List<OrderLine>
        {
            OrderLine.CreateProductLine(product.Id, product.Reference, product.Name, new OrderedQuantity(1),
                new ProductUnitPrice(1000), new VatRate(0))
        });

        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Where(p => p.SupplierId == supplier.Id)
            .Select(p => new ProductQuantityDto(p.Id.Value, 2))
            .ToList();

        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Id, lines),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
        Assert.AreEqual(2, order.Lines.First(l => l.Identifier == product.Id.Value).Quantity.Value);
    }

    [Test]
    public async Task Update_Only_With_Products_From_Order_Supplier()
    {
        var (accountId, context, handler) = InitHandler();

        var supplier = context.Suppliers.First(s => s.AccountId == accountId);
        var customer = context.Customers.First();

        var product = context.Products.First(o => o.SupplierId == supplier.Id);
        var order = Order.CreateDraft(supplier.Id, customer.Id);
        order.UpdateDraftLines(new List<OrderLine>
        {
            OrderLine.CreateProductLine(product.Id, product.Reference, product.Name, new OrderedQuantity(1),
                new ProductUnitPrice(1000), new VatRate(0))
        });

        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Select(p => new ProductQuantityDto(p.Id.Value, 2))
            .ToList();

        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Id, lines),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
        Assert.AreEqual(2, order.Lines.First(l => l.Identifier == product.Id.Value).Quantity.Value);
    }

    [Test]
    public async Task Remove_Lines_On_Order()
    {
        var (accountId, context, handler) = InitHandler();

        var supplier = context.Suppliers.First(s => s.AccountId == accountId);
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, true);

        context.Add(order);
        context.SaveChanges();

        var line = context.Products
            .Where(p => p.SupplierId == supplier.Id)
            .Select(p => new ProductQuantityDto(p.Id.Value, 2))
            .FirstOrDefault();

        var result =
            await handler.Handle(
                new UpdateOrderDraftProductsCommand(order.Id, new List<ProductQuantityDto> {line}),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, order.Lines.Count());
    }

    private (AccountId, AppDbContext, UpdateOrderDraftProductsHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<UpdateOrderDraftProductsHandler>();

        var supplier1Account  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplier1Account);
        
        var supplier2Account  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplier2Account);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        context.SaveChanges();

        var supplier1Agreements = new Dictionary<AccountId, DeliveryDay> {{customerAccount.Id, new DeliveryDay(DayOfWeek.Friday)}};
        var supplier1Products = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};
        var supplier2Products = new Dictionary<string, int> {{"T1", 1000}, {"T2", 5000}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAccount.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>>
                {{supplier1Account.Id, supplier1Agreements}, {supplier2Account.Id, new Dictionary<AccountId, DeliveryDay>()}},
            new Dictionary<AccountId, Dictionary<string, int>>
                {{supplier1Account.Id, supplier1Products}, {supplier2Account.Id, supplier2Products}});

        var handler = new UpdateOrderDraftProductsHandler(uow, new TransformProductsToOrderLines(context));

        return (supplier1Account.Id, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618