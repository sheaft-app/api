using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

public class CreateProductCommandShould
{
    [Test]
    public async Task Insert_Product()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var product = context.Products.Single(s => s.Identifier == new ProductId(result.Value) && s.SupplierIdentifier == supplierId);
        Assert.IsNotNull(product);
    }

    [Test]
    public async Task Insert_Product_If_Reference_Exists_From_Different_Supplier()
    {
        var (supplierId, context, handler) = InitHandler();
        context.Products.Add(new Product(new ProductName("tess"), new ProductReference("Code"), new VatRate(2000), null,
            SupplierId.New()));
        context.SaveChanges();
        var command = GetCommand(supplierId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var product = context.Products.Single(s => s.Identifier == new ProductId(result.Value));
        Assert.IsNotNull(product);
    }

    [Test]
    public async Task Generate_Reference_For_Product_If_Not_Provided()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, code: null);

        var result = await handler.Handle(command, CancellationToken.None);

        var product = context.Products.Single(s => s.Identifier == new ProductId(result.Value));
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("0000000000017", product.Reference.Value);
    }

    [Test]
    public async Task Insert_Product_With_Attached_Returnable()
    {
        var (supplierId, context, handler) = InitHandler();

        var returnableId = context.Returnables.First().Identifier;
        var command = GetCommand(supplierId, returnableId: returnableId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var product = context.Products.Single(s => s.Identifier == new ProductId(result.Value));
        Assert.IsNotNull(product?.Returnable);
        Assert.AreEqual(returnableId.Value, product?.Returnable.Identifier.Value);
    }

    [Test]
    public async Task Fail_To_Insert_Product_If_Returnable_Not_Found()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, returnableId: ReturnableId.New());

        var result = await handler.Handle(command, CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.NotFound, result.Error.Kind);
        Assert.AreEqual("returnable.not.found", result.Error.Code);
    }

    [Test]
    public async Task Fail_To_Insert_Product_If_New_Reference_Already_Exists()
    {
        var (supplierId, context, handler) = InitHandler();
        context.Products.Add(new Product(new ProductName("tess"), new ProductReference("Code"), new VatRate(2000), null,
            supplierId));
        
        context.SaveChanges();
        var command = GetCommand(supplierId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.Conflict, result.Error.Kind);
        Assert.AreEqual("product.code.already.exists", result.Error.Code);
    }

    [Test]
    public void Fail_To_Insert_Product_If_Price_Is_Equal_Zero()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, 0);

        Assert.That(() => handler.Handle(command, CancellationToken.None), Throws.InvalidOperationException);
    }

    private (SupplierId, AppDbContext, CreateProductHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateProductHandler>();

        var handler = new CreateProductHandler(
            uow,
            new RetrieveDefaultCatalog(new CatalogRepository(context)),
            new HandleProductCode(new ProductRepository(context), new GenerateProductCode(context)));

        var supplierId = SupplierId.New();

        context.Add(new Returnable(new ReturnableName("Test"), new ReturnableReference("code"), new UnitPrice(2000),
            new VatRate(2000), supplierId));

        context.SaveChanges();
        
        return (supplierId, context, handler);
    }

    private CreateProductCommand GetCommand(SupplierId supplierIdentifier, int price = 1200, string? code = "Code",
        ReturnableId returnableId = null)
    {
        var command = new CreateProductCommand("Test", code, "desc", price, 2000, returnableId, supplierIdentifier);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618