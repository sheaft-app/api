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
using Sheaft.Infrastructure.BillingManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class PublishInvoiceDraftCommandShould
{
    [Test]
    public async Task Set_Invoice_Status_As_Published_And_Generate_Reference()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Id);
    
        var result = await handler.Handle(command, CancellationToken.None);
    
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(InvoiceStatus.Published, invoice.Status);
        Assert.AreEqual(command.CreatedAt, invoice.PublishedOn);
        Assert.AreEqual("FCT2022-00001", invoice.Reference.Value);
    }
    
    [Test]
    public async Task Update_Customer_Billing_Info_If_They_Have_Changed_Since_Creation()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Id);
        var customer = context.Customers.Single(c => c.Id == invoice.Customer.Identifier);
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
    public async Task Set_Invoice_DueDate_In_30_Days()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Id);
    
        var result = await handler.Handle(command, CancellationToken.None);
    
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(invoice.DueDate);
        Assert.AreEqual(new InvoiceDueDate(command.CreatedAt.AddDays(30)), invoice.DueDate);
    }

    [Test]
    public async Task Fail_If_Invoice_Has_No_Lines()
    {
        var (invoice, context, handler) = InitHandler();
        var command = new PublishInvoiceDraftCommand(invoice.Id);
    
        var result = await handler.Handle(command, CancellationToken.None);
    
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.publish.requires.lines", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Invoice_Is_Not_A_Draft()
    {
        var (invoice, context, handler) = InitHandler(true);
        var command = new PublishInvoiceDraftCommand(invoice.Id);
        invoice.Publish(new InvoiceReference(0));
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
            new PublishInvoices(
                new InvoiceRepository(context),
                new GenerateInvoiceCode(context),
                new RetrieveBillingInformation(context)));

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);
        
        var customer = DataHelpers.GetDefaultCustomer(customerAccount.Id);
        context.Add(customer);

        var products = new List<InvoiceLine>();
        if (addProducts)
        {
            products = new List<InvoiceLine> {
                InvoiceLine.CreateLineForDeliveryOrder("Test1", "Name1", new Quantity(2), new UnitPrice(2000),
                    new VatRate(0), new InvoiceDelivery(new DeliveryReference(0), DateTimeOffset.UtcNow),
                    new DeliveryOrder(new OrderReference(0), DateTimeOffset.UtcNow)),
                InvoiceLine.CreateLineForDeliveryOrder("Test2", "Name2", new Quantity(1), new UnitPrice(2000),
                    new VatRate(0), new InvoiceDelivery(new DeliveryReference(0), DateTimeOffset.UtcNow),
                    new DeliveryOrder(new OrderReference(0), DateTimeOffset.UtcNow)),
            };
        }
        
        var invoice = Invoice.CreateInvoice(
            DataHelpers.GetDefaultSupplierBilling(supplier.Id),
            DataHelpers.GetDefaultCustomerBilling(customer.Id),
            products);

        context.Add(invoice);
        context.SaveChanges();
        return (invoice, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618