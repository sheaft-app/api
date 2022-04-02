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
    public async Task Add_Lines_To_Order()
    {
        var (context, handler) = InitHandler();
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier, customer.DeliveryAddress, new BillingAddress("", null, "", ""));
        context.Add(order);
        context.SaveChanges();

        var lines = context.Products
            .Where(p => p.SupplierIdentifier == supplier.Identifier)
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .ToList();
        
        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Identifier, lines), CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);
        
        Assert.IsNotNull(order);
        Assert.AreEqual(2, order.Lines.Count);
    }
    
    [Test]
    public async Task Remove_Lines_To_Order()
    {
        var (context, handler) = InitHandler();
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier, customer.DeliveryAddress,
            new BillingAddress("", null, "", ""));
        
        order.UpdateDraftLines(new List<OrderLine>
        {
            new OrderLine(new ProductId("test 1"), new ProductCode("test 1"), new ProductName("test 1"), new Quantity(1),
                new Price(2000), new VatRate(2000)),
            new OrderLine(new ProductId("test 2"), new ProductCode("test 2"), new ProductName("test 2"), new Quantity(1),
                new Price(2000), new VatRate(2000))
        });

        context.Add(order);
        context.SaveChanges();

        var line = context.Products
            .Where(p => p.SupplierIdentifier == supplier.Identifier)
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .FirstOrDefault();
        
        var result = await handler.Handle(new UpdateOrderDraftProductsCommand(order.Identifier, new List<ProductQuantityDto>{line}), CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);
        
        Assert.IsNotNull(order);
        Assert.AreEqual(1, order.Lines.Count);
    }

    private (AppDbContext, UpdateOrderDraftProductsHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<UpdateOrderDraftProductsHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};
        
        DataHelpers.InitContext(context,
            new List<AccountId>{customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});
        
        var handler = new UpdateOrderDraftProductsHandler(uow, new TransformProductsToOrderLines(context));
        
        return (context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
