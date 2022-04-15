using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.InvoiceManagement;
using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class UpdateInvoiceDraftCommandShould
{
    [Test]
    public async Task Add_Lines_To_Invoice()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new UpdateInvoiceDraftCommand(invoice.Identifier,
        new List<InvoiceLineDto>
            {
                new InvoiceLineDto("Name1", 2, 2000, 150),
                new InvoiceLineDto("Name2", 1, 2000, 3000)
            }, DateTimeOffset.UtcNow.AddDays(1));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, invoice.Lines.Count());
    }
    
    [Test]
    public async Task Update_Lines_On_Invoice()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new UpdateInvoiceDraftCommand(invoice.Identifier,
            new List<InvoiceLineDto>
            {
                new InvoiceLineDto("Name2", 1, 2000, 3000)
            },DateTimeOffset.UtcNow.AddDays(1));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, invoice.Lines.Count());
    }
    
    [Test]
    public async Task Remove_Lines_On_Invoice()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new UpdateInvoiceDraftCommand(invoice.Identifier, new List<InvoiceLineDto>(),DateTimeOffset.UtcNow.AddDays(1));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(0, invoice.Lines.Count());
    }
    
    [Test]
    public async Task Fail_If_Invoice_Is_Not_A_Draft()
    {
        var (invoice, context, handler) = InitHandler(true, true);
        var command = new UpdateInvoiceDraftCommand(invoice.Identifier, new List<InvoiceLineDto>(),DateTimeOffset.UtcNow.AddDays(1));
        invoice.Publish(new InvoiceReference("test"));
        context.SaveChanges();
        
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.update.lines.requires.draft", result.Error.Code);
    }

    private (Invoice, AppDbContext, UpdateInvoiceDraftHandler) InitHandler(bool addProducts = false, bool publish = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateInvoiceDraftHandler>();

        var handler = new UpdateInvoiceDraftHandler(uow);

        var supplierId = SupplierId.New();
        var customerId = CustomerId.New();

        if (addProducts)
        {
            var invoiceWithProducts = Invoice.CreateInvoiceDraftForOrder(DataHelpers.GetDefaultSupplierBilling(supplierId),
                DataHelpers.GetDefaultCustomerBilling(customerId), new List<LockedInvoiceLine>
                {
                    InvoiceLine.CreateLockedLine("Name1", new Quantity(2), new UnitPrice(2000), new VatRate(0)),
                    InvoiceLine.CreateLockedLine("Name2", new Quantity(1), new UnitPrice(2000), new VatRate(0))
                });

            invoiceWithProducts.SetDueOn(new InvoiceDueDate(DateTimeOffset.UtcNow.AddDays(1)));

            if(publish)
                invoiceWithProducts.Publish(new InvoiceReference("Test"));

            context.Add(invoiceWithProducts);
            context.SaveChanges();
            return (invoiceWithProducts, context, handler);
        }
        
        var invoice = Invoice.CreateInvoiceDraftForOrder(DataHelpers.GetDefaultSupplierBilling(supplierId),
            DataHelpers.GetDefaultCustomerBilling(customerId), new List<LockedInvoiceLine>());

        context.Add(invoice);
        context.SaveChanges();
        
        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618