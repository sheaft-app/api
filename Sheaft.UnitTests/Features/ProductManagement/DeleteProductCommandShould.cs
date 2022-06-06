using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

public class DeleteProductCommandShould
{
    [Test]
    public async Task Remove_Product_And_Update_Catalog()
    {
        var (productId, context, handler) = InitHandler();

        var result = await handler.Handle(new RemoveProductCommand(productId), CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        var product = context.Products.SingleOrDefault(s => s.Id == productId);
        var catalog = context.Catalogs
            .Include(c => c.Products)
            .ThenInclude(cp => cp.Product)
            .Single(s => s.IsDefault);
        Assert.IsNull(product);
        Assert.AreEqual(0, catalog.Products.Count);
    }
    
    [Test]
    public async Task Fail_To_Remove_Product_If_Identifier_Not_Exists()
    {
        var (productId, context, handler) = InitHandler();

        var result = await handler.Handle(new RemoveProductCommand(ProductId.New()), CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.NotFound, result.Error.Kind);
    }

    private (ProductId, AppDbContext, RemoveProductHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateProductHandler>();

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);
        
        var catalog = Catalog.CreateDefaultCatalog(supplier.Id);
        var product = new Product(new ProductName("product"), new ProductReference("test"),  new VatRate(0),null, supplier.Id, Maybe<Returnable>.None);
        catalog.AddOrUpdateProductPrice(product, new ProductUnitPrice(2000));

        context.Add(catalog);
        context.SaveChanges();
        
        var handler = new RemoveProductHandler(uow, new RetrieveDefaultCatalog(new CatalogRepository(context)));
        
        return (product.Id, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618
