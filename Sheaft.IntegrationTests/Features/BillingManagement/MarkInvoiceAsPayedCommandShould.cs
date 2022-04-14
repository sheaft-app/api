using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class MarkInvoiceAsPayedCommandShould
{
    [Test]
    public async Task Set_Invoice_Status_As_Payed()
    {
        var payedDate = DateTimeOffset.UtcNow.AddDays(-2);
        var (invoice, context, handler) = InitHandler(true, true);
        var command = new MarkInvoiceAsPayedCommand(invoice.Identifier, "VIRDFDFDF", payedDate, PaymentKind.Check);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Payed, invoice.Status);
        Assert.AreEqual(1, invoice.Payments.Count());
        
        var payment = invoice.Payments.Single();
        Assert.AreEqual(PaymentKind.Check, payment.Kind);
        Assert.AreEqual(payedDate, payment.PaymentDate);
    }
    
    [Test]
    public async Task Fail_If_Not_In_Sent_Status()
    {
        var (invoice, context, handler) = InitHandler(true, false);
        var command = new MarkInvoiceAsPayedCommand(invoice.Identifier, "test", DateTimeOffset.Now, PaymentKind.Check);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.payed.requires.sent", result.Error.Code);
    }

    private (Invoice, AppDbContext, MarkInvoiceAsPayedHandler) InitHandler(bool publish = true, bool sent = true)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<MarkInvoiceAsPayedHandler>();
        
        var handler = new MarkInvoiceAsPayedHandler(uow);

        var supplierId = SupplierId.New();
        var customerId = CustomerId.New();

        var invoice = Invoice.CreateInvoiceDraft(supplierId, customerId, DataHelpers.GetDefaultBilling());
        invoice.UpdateDraftLines(new List<InvoiceLine>
        {
            new InvoiceLine("Name1", new Quantity(2), new UnitPrice(2000), new VatRate(0)),
            new InvoiceLine("Name2", new Quantity(1), new UnitPrice(2000), new VatRate(0))
        });

        if (publish)
            invoice.Publish(new InvoiceReference("test"), invoice.Lines);
        
        if (sent)
            invoice.MarkAsSent();

        context.Add(invoice);
        context.SaveChanges();

        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618