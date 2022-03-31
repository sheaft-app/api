using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.SupplierManagement;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.SupplierManagement;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.SupplierManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class UpdateSupplierCommandShould
{
    [Test]
    public async Task Update_Supplier_Information()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId);

        var result = await handler.Handle(command, CancellationToken.None);

        var supplier = context.Suppliers.Single(s => s.Identifier == supplierId);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(supplier);
        Assert.AreEqual("TradeName", supplier.TradeName.Value);
    }

    [Test]
    public async Task Fail_To_Supplier_If_Not_Found()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(SupplierId.New());
        
        var result = await handler.Handle(command, CancellationToken.None);

        var supplier = context.Suppliers.Single(s => s.Identifier == supplierId);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNotNull(supplier);
        Assert.AreEqual("trade", supplier.TradeName.Value);
    }

    private (SupplierId, AppDbContext, UpdateSupplierHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateSupplierHandler>();
        var handler = new UpdateSupplierHandler(uow);

        var supplier = new Supplier(new TradeName("trade"), new EmailAddress("test@est.com"),
            new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), null, AccountId.New());
        
        context.Suppliers.Add(supplier);
        context.SaveChanges();
        
        return (supplier.Identifier, context, handler);
    }

    private static UpdateSupplierCommand GetCommand(SupplierId supplierIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var command = new UpdateSupplierCommand(supplierIdentifier, "TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address, address);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
