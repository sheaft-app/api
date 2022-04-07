using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.ProductManagement;

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

        var returnable = context.Returnables.Single(s => s.Identifier == new ReturnableId(result.Value));
        Assert.IsNotNull(returnable);
    }

    [Test]
    public async Task Fail_When_Inserting_Returnable_With_Duplicated_Reference()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, 1200, "Existing");

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
    }

    private (SupplierId, AppDbContext, CreateReturnableHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateReturnableHandler>();

        var handler = new CreateReturnableHandler(uow);

        var supplierId = SupplierId.New();

        context.Add(new Returnable(new ReturnableName("Test"), new ReturnableReference("Existing"), new UnitPrice(2000),
            new VatRate(2000), supplierId));

        context.SaveChanges();
        
        return (supplierId, context, handler);
    }

    private CreateReturnableCommand GetCommand(SupplierId supplierIdentifier, int price = 1200, string? code = "Code")
    {
        var command = new CreateReturnableCommand("Test", code, price, 2000, supplierIdentifier);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618