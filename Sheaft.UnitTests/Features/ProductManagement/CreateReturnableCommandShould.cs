using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.ProductManagement;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.ProductManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CreateReturnableCommandShould
{
    [Test]
    public async Task Insert_Returnable()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var returnable = context.Returnables.Single(s => s.Id == new ReturnableId(result.Value));
        Assert.IsNotNull(returnable);
    }

    [Test]
    public async Task Generate_Reference_For_Returnable_If_Not_Provided()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, code: null);

        var result = await handler.Handle(command, CancellationToken.None);

        var returnable = context.Returnables.Single(s => s.Id == new ReturnableId(result.Value));
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("1000000000016", returnable.Reference.Value);
    }

    [Test]
    public async Task Fail_To_Insert_Returnable_If_Reference_Already_Exists()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, 1200, "Existing");

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
    }

    private (SupplierId, AppDbContext, CreateReturnableHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateReturnableHandler>();

        var handler = new CreateReturnableHandler(new HandleReturnableCode(new ReturnableRepository(context), new GenerateReturnableCode(context)), uow);

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);

        context.Add(new Returnable(new ReturnableName("Test"), new ReturnableReference("Existing"), new UnitPrice(2000),
            new VatRate(0), supplier.Id));

        context.SaveChanges();
        
        return (supplier.Id, context, handler);
    }

    private CreateReturnableCommand GetCommand(SupplierId supplierIdentifier, int price = 1200, string? code = "Code")
    {
        var command = new CreateReturnableCommand("Test", code, price, 0, supplierIdentifier);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618