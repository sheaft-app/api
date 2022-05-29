using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RemoveInvoiceDraftCommandShould
{
    [Test]
    public async Task Remove_Draft_Invoice()
    {
        var (invoiceId, context, handler) = InitHandler();
        var command = new RemoveInvoiceDraftCommand(invoiceId);
    
        var result = await handler.Handle(command, CancellationToken.None);
    
        Assert.IsTrue(result.IsSuccess);
        
        var invoice = context.Invoices.SingleOrDefault(s => s.Id == invoiceId);
        Assert.IsNull(invoice);
    }

    [Test]
    public async Task Fail_If_Is_Not_A_Draft()
    {
        var (invoiceId, context, handler) = InitHandler(true);
        var command = new RemoveInvoiceDraftCommand(invoiceId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.remove.requires.draft", result.Error.Code);
    }

    private (InvoiceId, AppDbContext, RemoveInvoiceDraftHandler) InitHandler(bool publishCreditNote = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<RemoveInvoiceDraftHandler>();

        var handler = new RemoveInvoiceDraftHandler(uow);

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customerAccount.Id, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAccount.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierAccount.Id, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierAccount.Id, supplierProducts}});

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        context.Add(order);

        order.Publish(new OrderReference(0), order.Lines);

        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("test", new EmailAddress("ese@ese.com"), "street", "", "70000", "Test"),
            order.SupplierId, order.CustomerId, new List<Order> {order});

        context.Add(delivery);
        
        var invoice = Invoice.CreateInvoice(
            DataHelpers.GetDefaultSupplierBilling(supplier.Id),
            DataHelpers.GetDefaultCustomerBilling(customer.Id),
            new List<InvoiceLine>
            {
                InvoiceLine.CreateLineForDeliveryOrder("Test1", "Name1", new Quantity(2), new UnitPrice(2000),
                    new VatRate(0), new InvoiceDelivery(new DeliveryReference(0), DateTimeOffset.UtcNow),
                    new DeliveryOrder(new OrderReference(0), DateTimeOffset.UtcNow)),
                InvoiceLine.CreateLineForDeliveryOrder("Test2", "Name2", new Quantity(1), new UnitPrice(2000),
                    new VatRate(0), new InvoiceDelivery(new DeliveryReference(0), DateTimeOffset.UtcNow),
                    new DeliveryOrder(new OrderReference(0), DateTimeOffset.UtcNow)),
            });

        if (publishCreditNote)
            invoice.Publish(new InvoiceReference(0));
        
        context.Add(invoice);

        context.SaveChanges();
        return (invoice.Id, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618