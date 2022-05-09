using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.Models;
using Sheaft.Application.SupplierManagement;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.SupplierManagement;

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

    private (SupplierId, AppDbContext, UpdateSupplierHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateSupplierHandler>();
        var handler = new UpdateSupplierHandler(uow);

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        context.Suppliers.Add(supplier);
        context.SaveChanges();
        
        return (supplier.Identifier, context, handler);
    }

    private static UpdateSupplierCommand GetCommand(SupplierId supplierIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var namedAddress = new NamedAddressDto("ee", "tys@tese.com", "street", null, "74540", "city");
        var command = new UpdateSupplierCommand(supplierIdentifier, "TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address,namedAddress, namedAddress);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
