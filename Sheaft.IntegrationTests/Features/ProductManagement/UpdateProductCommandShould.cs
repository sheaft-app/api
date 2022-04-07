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
        Assert.AreEqual("newcode", product.Reference.Value);
        Assert.AreEqual("desc", product.Description);
        Assert.AreEqual(1000, catalog.Products.First(p => p.Product.Identifier == productId).UnitPrice.Value);
    }
    
    [Test]
    public async Task Update_Product_With_Generated_Code()
    {
        var (productId, context, handler) = InitHandler();
        var command = GetCommand(productId, code: null);

        var result = await handler.Handle(command, CancellationToken.None);

        var product = context.Products.Single(s => s.Identifier == productId);
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("0000000000017", product.Reference.Value);
    }

    [Test]
    public async Task Update_Product_With_Returnable()
    {
        var (productId, context, handler) = InitHandler();

        var returnableId = context.Returnables.First().Identifier;
        var command = GetCommand(productId, returnableId: returnableId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var product = context.Products.Single(s => s.Identifier == productId);
        Assert.IsNotNull(product?.Returnable);
        Assert.AreEqual(returnableId.Value, product?.Returnable.Identifier.Value);
    }

    [Test]
    public async Task Update_Product_Remove_Returnable_Link()
    {
        var (productId, context, handler) = InitHandler(true);

        var command = GetCommand(productId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var product = context.Products.Single(s => s.Identifier == productId);
        Assert.IsNull(product.Returnable);
    }

    [Test]
    public async Task Fail_Update_Product_With_Not_Found_Returnable()
    {
        var (productId, context, handler) = InitHandler();

        var command = GetCommand(productId, returnableId: ReturnableId.New());

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.NotFound, result.Error.Kind);
        Assert.AreEqual("returnable.not.found", result.Error.Code);
    }

    private (ProductId, AppDbContext, UpdateProductHandler) InitHandler(bool attachReturnable = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateProductHandler>();

        var supplierIdentifier = SupplierId.New();
        var catalog = Catalog.CreateDefaultCatalog(supplierIdentifier);
        var product = new Product(new ProductName("product"), new ProductReference("test"),  new VatRate(2000),null, supplierIdentifier);
        catalog.AddOrUpdateProductPrice(product, new ProductUnitPrice(2000));

        var returnable = new Returnable(new ReturnableName("Test"), new ReturnableReference("code"), new UnitPrice(2000),
            new VatRate(2000), supplierIdentifier);
        
        context.Add(returnable);
        context.Add(catalog);

        if (attachReturnable)
            product.SetReturnable(returnable);
        
        context.SaveChanges();
        
        var handler = new UpdateProductHandler(uow, new RetrieveDefaultCatalog(new CatalogRepository(context)), new HandleProductCode(new ProductRepository(context), new GenerateProductCode()));
        
        return (product.Identifier, context, handler);
    }

    private static UpdateProductCommand GetCommand(ProductId identifier, int price = 1200, string? code = "Code", ReturnableId? returnableId = null)
    {
        var command = new UpdateProductCommand(identifier, "Test", 2000, code, "desc", price, returnableId);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618
