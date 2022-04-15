using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.InvoiceManagement;
using Sheaft.Domain;
using Sheaft.Infrastructure.InvoiceManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CreateInvoiceDraftCommandShould
{
    [Test]
    public async Task Insert_Invoice_With_Status_As_Draft()
    {
        var (supplierId, customerId, context, handler) = InitHandler();
        var orders = context.Orders.Select(o => o.Identifier).ToList();
        var command = new CreateInvoiceDraftForOrdersCommand(orders);

        var result = await handler.Handle(command, CancellationToken.None);
        var newInvoiceIdentifiers = result.Value.Select(v => new InvoiceId(v)).ToList();
        
        Assert.IsTrue(result.IsSuccess);
        var invoice = context.Invoices.Single(s => newInvoiceIdentifiers.Contains(s.Identifier));
        Assert.IsNotNull(invoice);
        Assert.AreEqual(InvoiceStatus.Draft, invoice.Status);
    }

    private (SupplierId, CustomerId, AppDbContext, CreateInvoiceDraftForOrdersHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateInvoiceDraftForOrdersHandler>();

        var handler = new CreateInvoiceDraftForOrdersHandler(uow, new RetrieveBillingInformation(context));

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        context.Add(supplier);

        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        context.Add(customer);

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        context.Add(order);
        
        context.SaveChanges();
        
        return (supplier.Identifier, customer.Identifier, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618