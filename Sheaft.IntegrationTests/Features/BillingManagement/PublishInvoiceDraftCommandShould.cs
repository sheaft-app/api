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

public class PublishInvoiceDraftCommandShould
{
    [Test]
    public async Task Set_Invoice_Status_As_Published_And_Generate_Reference()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Identifier, DateTimeOffset.UtcNow.AddDays(1));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Published, invoice.Status);
        Assert.AreEqual(command.CreatedAt, invoice.PublishedOn);
        Assert.AreEqual("0000001", invoice.Reference.Value);
    }
    
    [Test]
    public async Task Update_Customer_Billing_Info_If_They_Have_Changed_Since_Creation()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Identifier, DateTimeOffset.UtcNow.AddDays(1));
        var customer = context.Customers.Single(c => c.Identifier == invoice.Customer.Identifier);
        customer.SetBillingAddress(new BillingAddress("update", new EmailAddress("test@est.com"), "New street", "",
            "7000", "city"));

        context.SaveChanges();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Published, invoice.Status);
        Assert.AreEqual("update", invoice.Customer.Name);
        Assert.AreEqual("New street", invoice.Customer.Address.Street);
    }

    [Test]
    public async Task Fail_If_Invoice_Has_No_Lines()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new PublishInvoiceDraftCommand(invoice.Identifier, DateTimeOffset.UtcNow.AddDays(2),new List<InvoiceLineDto>());

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.publish.requires.lines", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Invoice_Is_Not_A_Draft()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Identifier, DateTimeOffset.UtcNow.AddDays(1));
        invoice.Publish(new InvoiceReference("test"), new InvoiceDueDate(DateTimeOffset.UtcNow.AddDays(1)), invoice.Lines);
        context.SaveChanges();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.publish.requires.draft", result.Error.Code);
    }

    private (Invoice, AppDbContext, PublishInvoiceDraftHandler) InitHandler(bool addProducts = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<PublishInvoiceDraftHandler>();

        var handler = new PublishInvoiceDraftHandler(uow,
            new PublishOrders(
                new InvoiceRepository(context), 
                new GenerateInvoiceCode(), 
                new RetrieveBillingInformation(context)));

        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        context.Add(customer);
        
        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        context.Add(supplier);

        var invoice = Invoice.CreateInvoiceDraft(DataHelpers.GetDefaultSupplierBilling(supplier.Identifier), DataHelpers.GetDefaultCustomerBilling(customer.Identifier));

        if (addProducts)
            invoice.UpdateDraftLines(new List<InvoiceLine>
            {
                new InvoiceLine("Name1", new Quantity(2), new UnitPrice(2000), new VatRate(0)),
                new InvoiceLine("Name2", new Quantity(1), new UnitPrice(2000), new VatRate(0))
            });

        context.Add(invoice);
        context.SaveChanges();

        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618