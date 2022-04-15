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
        var command = new CreateInvoiceDraftCommand(supplierId, customerId);

        var result = await handler.Handle(command, CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        var invoice = context.Invoices.Single(s => s.Identifier == new InvoiceId(result.Value));
        Assert.IsNotNull(invoice);
        Assert.AreEqual(InvoiceStatus.Draft, invoice.Status);
    }

    private (SupplierId, CustomerId, AppDbContext, CreateInvoiceDraftHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateInvoiceDraftHandler>();

        var handler = new CreateInvoiceDraftHandler(uow, new RetrieveBillingInformation(context));

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        context.Add(supplier);

        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        context.Add(customer);
        context.SaveChanges();
        
        return (supplier.Identifier, customer.Identifier, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618