using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.InvoiceManagement;
using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.InvoiceManagement;
using Sheaft.Infrastructure.InvoiceManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CancelInvoiceCommandShould
{
    [Test]
    public async Task Set_Invoice_Status_As_Cancelled()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new CancelInvoiceCommand(invoice.Identifier, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Cancelled, invoice.Status);
        Assert.AreEqual(command.CreatedAt, invoice.CancelledOn);
        Assert.AreEqual("Reason", invoice.CancellationReason);
    }
    
    [Test]
    public async Task Generate_CreditNote_With_Amount_Equal_To_Invoice()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new CancelInvoiceCommand(invoice.Identifier, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess); 
        Assert.IsNotEmpty(invoice.CreditNotes);
        Assert.AreNotEqual(invoice.Identifier, new InvoiceId(result.Value));

        var creditNote = context.Invoices.Single(i => i.Identifier == new InvoiceId(result.Value));
        Assert.IsNotNull(creditNote);
        Assert.AreEqual(InvoiceKind.CreditNote, creditNote.Kind);
        Assert.AreEqual(1, creditNote.Lines.Count());
        Assert.AreEqual(invoice.TotalWholeSalePrice, creditNote.TotalWholeSalePrice);
        Assert.AreEqual(invoice.TotalVatPrice, creditNote.TotalVatPrice);
        Assert.AreEqual(invoice.TotalOnSalePrice, creditNote.TotalOnSalePrice);
    }

    [Test]
    public async Task Fail_If_Invoice_Is_Already_Payed()
    {
        var (invoice, context, handler) = InitHandler(true, true, true);
        var command = new CancelInvoiceCommand(invoice.Identifier, "Reason");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.cancel.requires.published.or.sent", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Reason_Is_Not_Provided()
    {
        var (invoice, context, handler) = InitHandler(true, false);
        var command = new CancelInvoiceCommand(invoice.Identifier, "");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.cancel.requires.reason", result.Error.Code);
    }

    private (Invoice, AppDbContext, CancelInvoiceHandler) InitHandler(bool publish = true, bool sent = true, bool payed = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CancelInvoiceHandler>();

        var handler = new CancelInvoiceHandler(uow, new CancelInvoices(new InvoiceRepository(context), new GenerateCreditNoteCode()));

        var supplierId = SupplierId.New();
        var customerId = CustomerId.New();

        var invoice = Invoice.CreateInvoiceDraft(DataHelpers.GetDefaultSupplierBilling(supplierId), DataHelpers.GetDefaultCustomerBilling(customerId));
        invoice.UpdateDraftLines(new List<InvoiceLine>
        {
            new InvoiceLine("Name1", new Quantity(2), new UnitPrice(2000), new VatRate(0)),
            new InvoiceLine("Name2", new Quantity(1), new UnitPrice(2000), new VatRate(0))
        });

        if (publish)
            invoice.Publish(new InvoiceReference("test"), new InvoiceDueDate(DateTimeOffset.UtcNow.AddDays(2)), invoice.Lines);
        
        if (sent)
            invoice.MarkAsSent();

        if (payed)
            invoice.MarkAsPayed("", PaymentKind.Check, DateTimeOffset.Now);

        context.Add(invoice);
        context.SaveChanges();

        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618