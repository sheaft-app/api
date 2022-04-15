using System;
using System.Collections.Generic;
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

public class SendInvoiceCommandShould
{
    [Test]
    public async Task Set_Invoice_Status_As_Sent()
    {
        var (invoice, context, handler) = InitHandler(true, false);
        var command = new SendInvoiceCommand(invoice.Identifier);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Sent, invoice.Status);
        Assert.AreEqual(command.CreatedAt, invoice.SentOn);
    }
    
    [Test]
    public async Task Fail_If_Not_In_Published_Status()
    {
        var (invoice, context, handler) = InitHandler(false, false);
        var command = new SendInvoiceCommand(invoice.Identifier);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.sent.requires.published", result.Error.Code);
    }

    private (Invoice, AppDbContext, SendInvoiceHandler) InitHandler(bool publish = true, bool sent = true)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<SendInvoiceHandler>();

        var emailServiceMocked = new Mock<IEmailingService>();
        emailServiceMocked
            .Setup(c => c.SendTemplatedEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Result.Success()));
        
        var handler = new SendInvoiceHandler(uow, emailServiceMocked.Object);

        var supplierId = SupplierId.New();
        var customerId = CustomerId.New();

        var invoice = Invoice.CreateInvoiceDraftForOrder(DataHelpers.GetDefaultSupplierBilling(supplierId), DataHelpers.GetDefaultCustomerBilling(customerId),
            new List<OrderToInvoice>
            {
                new OrderToInvoice(new OrderReference("Ref"), DateTimeOffset.Now, new List<LockedInvoiceLine>
                {
                    InvoiceLine.CreateLockedLine("Name1", new Quantity(2), new UnitPrice(2000), new VatRate(0)),
                    InvoiceLine.CreateLockedLine("Name2", new Quantity(1), new UnitPrice(2000), new VatRate(0))
                })
            });

        invoice.UpdatePaymentInformation(new InvoiceDueDate(DateTimeOffset.UtcNow.AddDays(1)));

        if (publish)
            invoice.Publish(new InvoiceReference("test"));
        
        if (sent)
            invoice.MarkAsSent();

        context.Add(invoice);
        context.SaveChanges();

        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618