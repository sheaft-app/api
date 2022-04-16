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

public class RemoveInvoiceDraftCommandShould
{
    // [Test]
    // public async Task Remove_Draft_Invoice()
    // {
    //     var (invoiceId, context, handler) = InitHandler();
    //     var command = new RemoveInvoiceDraftCommand(invoiceId);
    //
    //     var result = await handler.Handle(command, CancellationToken.None);
    //
    //     Assert.IsTrue(result.IsSuccess);
    //     var invoice = context.Invoices.SingleOrDefault(s => s.Identifier == invoiceId);
    //     Assert.IsNull(invoice);
    // }

    [Test]
    public async Task Fail_If_Not_A_Draft()
    {
        var (deliveryId, context, handler) = InitHandler(true);
        var command = new RemoveInvoiceDraftCommand(deliveryId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.remove.requires.draft", result.Error.Code);
    }

    private (InvoiceId, AppDbContext, RemoveInvoiceDraftHandler) InitHandler(bool publishInvoice = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<RemoveInvoiceDraftHandler>();

        var handler = new RemoveInvoiceDraftHandler(uow);

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

        context.Add(delivery);

        var invoice = Invoice.CreateInvoiceForOrder(
            DataHelpers.GetDefaultSupplierBilling(supplier.Identifier),
            DataHelpers.GetDefaultCustomerBilling(customer.Identifier),
            new List<InvoiceLine>
            {
                InvoiceLine.CreateLineForDeliveryOrder("Test1", "Name1", new Quantity(2), new UnitPrice(2000),
                    new VatRate(0), new InvoiceDelivery(new DeliveryReference("Test"), DateTimeOffset.UtcNow),
                    new DeliveryOrder(new OrderReference("Test"), DateTimeOffset.UtcNow)),
                InvoiceLine.CreateLineForDeliveryOrder("Test2", "Name2", new Quantity(1), new UnitPrice(2000),
                    new VatRate(0), new InvoiceDelivery(new DeliveryReference("Test"), DateTimeOffset.UtcNow),
                    new DeliveryOrder(new OrderReference("Test"), DateTimeOffset.UtcNow)),
            }, new InvoiceReference("Test"));

        if (publishInvoice)
            invoice.Publish(new InvoiceReference("test"));

        context.Add(invoice);

        context.SaveChanges();
        return (invoice.Identifier, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618