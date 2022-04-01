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

public class DeleteProductCommandShould
{
    [Test]
    public async Task Remove_Product()
    {
        var (productId, context, handler) = InitHandler();

        var result = await handler.Handle(new RemoveProductCommand(productId), CancellationToken.None);

        var product = context.Products.SingleOrDefault(s => s.Identifier == productId);
        var catalog = context.Catalogs
            .Include(c => c.Products)
            .ThenInclude(cp => cp.Product)
            .Single(s => s.IsDefault);
        
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(product);
        Assert.AreEqual(0, catalog.Products.Count);
    }

    private (ProductId, AppDbContext, RemoveProductHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateProductHandler>();

        var supplierIdentifier = SupplierId.New();
        var catalog = Catalog.CreateDefaultCatalog(supplierIdentifier);
        var product = new Product(new ProductName("product"), new ProductCode("test"), null, supplierIdentifier);
        catalog.AddOrUpdateProductPrice(product, new ProductPrice(2000));

        context.Add(catalog);
        context.SaveChanges();
        
        var handler = new RemoveProductHandler(uow, new RetrieveDefaultCatalog(new CatalogRepository(context)));
        
        return (product.Identifier, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618
