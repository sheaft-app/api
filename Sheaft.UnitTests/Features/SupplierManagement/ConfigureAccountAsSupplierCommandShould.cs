using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.Models;
using Sheaft.Application.SupplierManagement;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.SupplierManagement;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.SupplierManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ConfigureAccountAsSupplierCommandShould
{
    [Test]
    public async Task Create_Supplier_For_Specified_AccountIdentifier()
    {
        var (context, handler) = InitHandler();
        var accountIdentifier = AccountId.New();
        var command = GetCommand(accountIdentifier);

        var result = await handler.Handle(command, CancellationToken.None);

        var supplier = context.Suppliers.SingleOrDefault(s => s.AccountIdentifier == accountIdentifier);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(supplier);
    }

    [Test]
    public async Task Fail_To_Create_Supplier_If_AccountIdentifier_Already_Registered()
    {
        var (context, handler) = InitHandler();
        var accountIdentifier = AccountId.New();
        
        context.Suppliers.Add(DataHelpers.GetDefaultSupplier(accountIdentifier));
        await context.SaveChangesAsync();
        var command = GetCommand(accountIdentifier);
        
        var result = await handler.Handle(command, CancellationToken.None);

        var supplier = context.Suppliers.Single(s => s.AccountIdentifier == accountIdentifier);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNotNull(supplier);
    }

    private (AppDbContext, ConfigureAccountAsSupplierHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ConfigureAccountAsSupplierHandler>();
        var handler = new ConfigureAccountAsSupplierHandler(uow, new ValidateSupplierRegistration(context));
        
        return (context, handler);
    }

    private static ConfigureAccountAsSupplierCommand GetCommand(AccountId accountIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var namedAddress = new NamedAddressDto("ee", "tys@tese.com", "street", null, "74540", "city");
        var command = new ConfigureAccountAsSupplierCommand("TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address, namedAddress, namedAddress, accountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618