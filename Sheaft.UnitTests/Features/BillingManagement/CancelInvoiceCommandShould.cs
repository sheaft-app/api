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
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.BillingManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CancelInvoiceCommandShould
{
    [Test]
    public async Task Set_Invoice_Status_As_Cancelled()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new CancelInvoiceCommand(invoice.Id, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Cancelled, invoice.Status);
        Assert.AreEqual(command.CreatedAt, invoice.CancelledOn);
        Assert.AreEqual("Reason", invoice.CancellationReason);
    }

    [Test]
    public async Task Remove_InvoiceIdentifier_On_Orders()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new CancelInvoiceCommand(invoice.Id, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var order = context.Orders.SingleOrDefault(s => s.InvoiceId == invoice.Id);
        Assert.IsNull(order);
    }

    [Test]
    public async Task Generate_CreditNote_With_Amount_Equal_To_Invoice()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new CancelInvoiceCommand(invoice.Id, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotEmpty(invoice.CreditNotes);
        Assert.AreNotEqual(invoice.Id, new InvoiceId(result.Value));

        var creditNote = context.Invoices.Single(i => i.Id == new InvoiceId(result.Value));
        Assert.IsNotNull(creditNote);
        Assert.AreEqual(invoice.TotalWholeSalePrice, creditNote.TotalWholeSalePrice);
        Assert.AreEqual(invoice.TotalVatPrice, creditNote.TotalVatPrice);
        Assert.AreEqual(invoice.TotalOnSalePrice, creditNote.TotalOnSalePrice);
    }

    [Test]
    public async Task Generate_Published_CreditNote()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new CancelInvoiceCommand(invoice.Id, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var creditNote = context.Invoices.Single(i => i.Id == new InvoiceId(result.Value));
        Assert.IsNotNull(creditNote);
        Assert.AreEqual(InvoiceKind.InvoiceCancellation, creditNote.Kind);
        Assert.AreEqual(InvoiceStatus.Published, creditNote.Status);
    }

    [Test]
    public async Task Fail_If_Invoice_Is_Already_Payed()
    {
        var (invoice, context, handler) = InitHandler(true, true, true);
        var command = new CancelInvoiceCommand(invoice.Id, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.cancel.requires.published.or.sent", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Reason_Is_Not_Provided()
    {
        var (invoice, context, handler) = InitHandler(true, false);
        var command = new CancelInvoiceCommand(invoice.Id, "");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.cancel.requires.reason", result.Error.Code);
    }

    private (Invoice, AppDbContext, CancelInvoiceHandler) InitHandler(bool publish = true, bool sent = true,
        bool payed = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CancelInvoiceHandler>();

        var handler = new CancelInvoiceHandler(uow,
            new CancelInvoices(new InvoiceRepository(context), new OrderRepository(context),
                new GenerateCreditNoteCode(context)));

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);
        
        var customer = DataHelpers.GetDefaultCustomer(customerAccount.Id);
        context.Add(customer);

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false,
            new List<Product>
                {new Product(new ProductName("test"), new ProductReference("code"), new VatRate(0), null, supplier.Id)});

        var invoice = Invoice.CreateInvoiceForOrder(
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
            }, new InvoiceReference(0));

        order.AttachInvoice(invoice.Id);

        if (publish)
            invoice.Publish(new InvoiceReference(0));

        if (sent)
            invoice.MarkAsSent();

        if (payed)
            invoice.MarkAsPayed(new PaymentReference("test"), PaymentKind.Check, DateTimeOffset.Now);

        context.Add(order);
        context.Add(invoice);
        context.SaveChanges();

        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618