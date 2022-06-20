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

public class UpdateReturnableCommandShould
{
    [Test]
    public async Task Set_Reference()
    {
        var (returnableId, context, handler) = InitHandler();
        var command = GetCommand(returnableId);

        var result = await handler.Handle(command, CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        var returnable = context.Returnables.Single(s => s.Id == returnableId);
        Assert.IsNotNull(returnable);
        Assert.AreEqual("CODE", returnable.Reference.Value);
    }

    [Test]
    public async Task Fail_To_Update_If_New_Reference_Already_Exists()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, 1200, "Existing");

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
    }

    private (ReturnableId, AppDbContext, UpdateReturnableHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateReturnableHandler>();

        var handler = new UpdateReturnableHandler(new HandleReturnableCode(new ReturnableRepository(context), new GenerateReturnableCode(context)), uow);

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);

        var returnable = new Returnable(new ReturnableName("Test1"), new ReturnableReference("Test1"), new UnitPrice(2000),
            new VatRate(0), supplier.Id);
        context.Add(returnable);
        
        var returnable2 = new Returnable(new ReturnableName("Test2"), new ReturnableReference("Existing"), new UnitPrice(2000),
            new VatRate(0), supplier.Id);
        context.Add(returnable2);

        context.SaveChanges();
        
        return (returnable.Id, context, handler);
    }

    private UpdateReturnableCommand GetCommand(ReturnableId returnableId, int price = 1200, string? code = "Code")
    {
        var command = new UpdateReturnableCommand(SupplierId.New(), returnableId, "Test", code, price, 0);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618