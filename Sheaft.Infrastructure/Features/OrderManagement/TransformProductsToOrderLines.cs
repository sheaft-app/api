using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class TransformProductsToOrderLines : ITransformProductsToOrderLines
{
    private readonly IDbContext _context;

    public TransformProductsToOrderLines(IDbContext context)
    {
        _context = context;
    }
    public async Task<Result<IEnumerable<OrderLine>>> Transform(IEnumerable<ProductsQuantities> products, SupplierId supplierIdentifier, CancellationToken token)
    {
        try
        {
            var catalog = await _context.Set<Catalog>()
                .Include(c => c.Products)
                    .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(c => c.SupplierIdentifier == supplierIdentifier && c.IsDefault, token);
            
            if (catalog == null)
                return Result.Failure<IEnumerable<OrderLine>>(ErrorKind.NotFound, "supplier.catalog.not.found");

            var productsIdentifiers = products
                .Select(p => p.ProductIdentifier)
                .ToList();
            
            var productsToTransform = catalog.Products
                .Where(cp => productsIdentifiers.Contains(cp.Product.Identifier));
            
            var lines = productsToTransform
                .Select(cp => 
                    new OrderLine(
                        cp.Product.Identifier, 
                        cp.Product.Code, 
                        cp.Product.Name, 
                        GetProductQuantity(cp.Product.Identifier, products), 
                        cp.Price, 
                        cp.Product.Vat));

            return Result.Success(lines);
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<OrderLine>>(ErrorKind.Unexpected, "database.error");
        }
    }

    private Quantity GetProductQuantity(ProductId productIdentifier,
        IEnumerable<ProductsQuantities> productsQuantities)
    {
        var productQuantity = productsQuantities.SingleOrDefault(p => p.ProductIdentifier == productIdentifier);
        return productQuantity == null ? new Quantity(0) : productQuantity.Quantity;
    }
}