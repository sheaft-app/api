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
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.ProductManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class DeleteReturnableCommandShould
{
    [Test]
    public async Task Remove_Returnable()
    {
        var (productId, context, handler) = InitHandler();

        var result = await handler.Handle(new RemoveReturnableCommand(productId), CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        
        var returnable = context.Returnables.SingleOrDefault(s => s.Identifier == productId);
        Assert.IsNull(returnable);
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
