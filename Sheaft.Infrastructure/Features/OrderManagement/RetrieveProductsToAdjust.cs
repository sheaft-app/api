using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class RetrieveProductsToAdjust : IRetrieveProductsToAdjust
{
    private readonly IDbContext _context;

    public RetrieveProductsToAdjust(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<DeliveryLine>>> Get(SupplierId supplierIdentifier,
        IEnumerable<ProductAdjustment> productAdjustments,
        CancellationToken token)
    {
        try
        {
            var catalog = await _context
                .Set<Catalog>()
                .Include(c => c.Products)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.Returnable)
                .SingleOrDefaultAsync(c => c.IsDefault && c.SupplierIdentifier == supplierIdentifier, token);

            var productsToAdjust = productAdjustments.Select(p => p.Identifier).ToList();
            var products = catalog.Products.Where(p => productsToAdjust.Contains(p.Product.Identifier))
                .ToList();
            
            if (products.Count != productAdjustments.Count())
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.BadRequest,
                    "delivery.products.to.adjust.not.found");

            var adjustedProducts = products.Select(p =>
                DeliveryLine.CreateProductLine(p.Product.Identifier, p.Product.Code, p.Product.Name,
                    GetProductQuantity(p.Product.Identifier, productAdjustments), p.UnitPrice,
                    p.Product.Vat)).ToList();
            
            adjustedProducts.AddRange(products
                .Where(p => p.Product.Returnable != null)
                .Select(p =>
                    DeliveryLine.CreateReturnableLine(p.Product.Returnable.Identifier, p.Product.Returnable.Reference, p.Product.Returnable.Name,
                        GetProductQuantity(p.Product.Identifier, productAdjustments), p.Product.Returnable.UnitPrice,
                        p.Product.Returnable.Vat)));

            return Result.Success(adjustedProducts.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.Unexpected, "database.error");
        }
    }

    private Quantity GetProductQuantity(ProductId productIdentifier, IEnumerable<ProductAdjustment> productAdjustments)
    {
        return productAdjustments.Single(p => p.Identifier == productIdentifier).Quantity;
    }
}