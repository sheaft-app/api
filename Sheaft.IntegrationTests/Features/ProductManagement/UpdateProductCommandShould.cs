using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

public class UpdateProductCommandShould
{
    [Test]
    public async Task Update_Product()
    {
        var (productId, context, handler) = InitHandler();
        var command = GetCommand(productId, 1000, "newcode");

        var result = await handler.Handle(command, CancellationToken.None);

        var product = context.Products.Single(s => s.Identifier == productId);
        var catalog = context.Catalogs.Single(c => c.Products.Any(cp => cp.Product.Identifier == productId));
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("newcode", product.Code.Value);
        Assert.AreEqual("desc", product.Description);
        Assert.AreEqual(1000, catalog.Products.First(p => p.Product.Identifier == productId).Price.Value);
    }
    
    [Test]
    public async Task Update_Product_With_Generated_Code()
    {
        var (productId, context, handler) = InitHandler();
        var command = GetCommand(productId, code: null);

        var result = await handler.Handle(command, CancellationToken.None);

        var product = context.Products.Single(s => s.Identifier == productId);
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("0000000000017", product.Code.Value);
    }

    private (ProductId, AppDbContext, UpdateProductHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateProductHandler>();

        var supplierIdentifier = SupplierId.New();
        var catalog = Catalog.CreateDefaultCatalog(supplierIdentifier);
        var product = new Product(new ProductName("product"), new ProductCode("test"), null, supplierIdentifier);
        catalog.AddOrUpdateProductPrice(product, new ProductPrice(2000));

        context.Add(catalog);
        context.SaveChanges();
        
        var handler = new UpdateProductHandler(uow, new RetrieveDefaultCatalog(new CatalogRepository(context)), new HandleProductCode(new ProductRepository(context), new GenerateProductCode()));
        
        return (product.Identifier, context, handler);
    }

    private static UpdateProductCommand GetCommand(ProductId identifier, int price = 1200, string? code = "Code")
    {
        var command = new UpdateProductCommand(identifier, "Test", code, "desc", price);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618
