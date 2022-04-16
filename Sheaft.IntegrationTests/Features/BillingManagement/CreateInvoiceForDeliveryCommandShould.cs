using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.BillingManagement;
using Sheaft.Application.InvoiceManagement;
using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.InvoiceManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.InvoiceManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CreateInvoiceForDeliveryCommandShould
{
    [Test]
    public async Task Insert_Invoice_With_Status_Published()
    {
        var (deliveryId, context, handler) = InitHandler();
        var command = new CreateInvoiceForDeliveryCommand(deliveryId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var invoice = context.Invoices.Single(s => s.Identifier == new InvoiceId(result.Value));
        Assert.IsNotNull(invoice);
        Assert.AreEqual(InvoiceStatus.Published, invoice.Status);
    }

    [Test]
    public async Task Assign_InvoiceIdentifier_To_Delivery_Orders()
    {
        var (deliveryId, context, handler) = InitHandler();
        var command = new CreateInvoiceForDeliveryCommand(deliveryId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var order = context.Orders.Single(s => s.InvoiceIdentifier == new InvoiceId(result.Value));
        Assert.IsNotNull(order);
    }

    private (DeliveryId, AppDbContext, CreateInvoiceForDeliveryHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateInvoiceForDeliveryHandler>();

        var handler = new CreateInvoiceForDeliveryHandler(
            uow,
            new CreateInvoices(
                new InvoiceRepository(context), 
                new DeliveryRepository(context),
                new OrderRepository(context), 
                new GenerateInvoiceCode(),
                new RetrieveBillingInformation(context)));

        var supplierAccount = AccountId.New();
        var customerAccount = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customerAccount, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAccount},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierAccount, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierAccount, supplierProducts}});

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        context.Add(order);

        order.Publish(new OrderReference("test"), order.Lines);

        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("test", new EmailAddress("ese@ese.com"), "street", "", "70000", "Test"),
            order.SupplierIdentifier, order.CustomerIdentifier, new List<Order> {order});

        var product = context.Products.First();
        delivery.UpdateLines(new List<DeliveryLine>
        {
            DeliveryLine.CreateProductLine(product.Identifier, product.Reference, product.Name, new Quantity(2), new ProductUnitPrice(2000),
                new VatRate(0), new DeliveryOrder(new OrderReference("Test"), DateTimeOffset.UtcNow), null)
        });

        delivery.Schedule(new DeliveryReference("Test"), new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)));
        delivery.Deliver(new List<DeliveryLine>());
        
        context.Add(delivery);

        context.SaveChanges();

        return (delivery.Identifier, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618