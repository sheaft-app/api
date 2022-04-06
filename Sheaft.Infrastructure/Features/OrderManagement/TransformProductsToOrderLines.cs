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
            var productsToProcess = products.Where(p => p.Quantity.Value > 0);
            if (!productsToProcess.Any())
                return Result.Success(new List<OrderLine>().AsEnumerable());
            
            var catalog = await _context.Set<Catalog>()
                .Include(c => c.Products)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.Returnable)
                .SingleOrDefaultAsync(c => c.SupplierIdentifier == supplierIdentifier && c.IsDefault, token);
            
            if (catalog == null)
                return Result.Failure<IEnumerable<OrderLine>>(ErrorKind.NotFound, "supplier.catalog.not.found");

            var productsIdentifiers = productsToProcess
                .Select(p => p.ProductIdentifier)
                .ToList();
            
            var productsToTransform = catalog.Products
                .Where(cp => productsIdentifiers.Contains(cp.Product.Identifier));
            
            var lines = new List<OrderLine>();
            
            lines.AddRange(productsToTransform
                .Select(cp => 
                    OrderLine.CreateProductLine(
                        cp.Product.Identifier, 
                        cp.Product.Code, 
                        cp.Product.Name, 
                        GetProductQuantity(cp.Product.Identifier, productsToProcess), 
                        cp.Price, 
                        cp.Product.Vat)));
            
            lines.AddRange(productsToTransform
                .Where(p => p.Product.Returnable != null)
                .Select(cp => 
                    OrderLine.CreateReturnableLine(
                        cp.Product.Returnable.Identifier, 
                        cp.Product.Returnable.Reference, 
                        cp.Product.Returnable.Name, 
                        GetProductQuantity(cp.Product.Identifier, productsToProcess), 
                        cp.Product.Returnable.Price, 
                        cp.Product.Returnable.Vat)));

            return Result.Success(lines.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<OrderLine>>(ErrorKind.Unexpected, "database.error");
        }
    }

    private OrderedQuantity GetProductQuantity(ProductId productIdentifier,
        IEnumerable<ProductsQuantities> productsQuantities)
    {
        var productQuantity = productsQuantities.SingleOrDefault(p => p.ProductIdentifier == productIdentifier);
        return productQuantity == null ? new OrderedQuantity(0) : productQuantity.Quantity;
    }
}