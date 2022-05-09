using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.ProductManagement;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.ProductManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class DeleteReturnableCommandShould
{
    [Test]
    public async Task Remove_Returnable()
    {
        var (returnableId, context, handler) = InitHandler();

        var result = await handler.Handle(new RemoveReturnableCommand(returnableId), CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        
        var returnable = context.Returnables.SingleOrDefault(s => s.Identifier == returnableId);
        Assert.IsNull(returnable);
    }
    
    [Test]
    public async Task Fail_To_Remove_Returnable_If_Identifier_Not_Exists()
    {
        var (productId, context, handler) = InitHandler();

        var result = await handler.Handle(new RemoveReturnableCommand(ReturnableId.New()), CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.NotFound, result.Error.Kind);
    }

    private (ReturnableId, AppDbContext, RemoveReturnableHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateProductHandler>();

        var supplierIdentifier = SupplierId.New();
        
        var returnable = new Returnable(new ReturnableName("Test1"), new ReturnableReference("Test1"), new UnitPrice(2000),
            new VatRate(2000), supplierIdentifier);
        context.Add(returnable);

        context.SaveChanges();
        
        var handler = new RemoveReturnableHandler(uow);
        
        return (returnable.Identifier, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618
