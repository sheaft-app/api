using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class CreateDeliveryProductAdjustments : ICreateDeliveryProductAdjustments
{
    private readonly IDbContext _context;

    public CreateDeliveryProductAdjustments(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<DeliveryLine>>> Get(Delivery delivery,
        IEnumerable<ProductAdjustment> productAdjustments, CancellationToken token)
    {
        try
        {
            var catalog = await _context
                .Set<Catalog>()
                .Include(c => c.Products)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.Returnable)
                .SingleOrDefaultAsync(c => c.IsDefault && c.SupplierIdentifier == delivery.SupplierIdentifier, token);

            var productsToAdjust = productAdjustments.Select(p => p.Identifier).ToList();
            var products = catalog.Products.Where(p => productsToAdjust.Contains(p.Product.Identifier))
                .ToList();
            
            if (products.Count != productAdjustments.Count())
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.BadRequest,
                    "delivery.products.to.adjust.not.found");

            var adjustedProducts = products.Select(p =>
                {
                    var existingProduct =
                        delivery.Lines.SingleOrDefault(dl => dl.Identifier == p.Product.Identifier.Value);

                    var price = p.UnitPrice;
                    var vat = p.Product.Vat;
                    var name = p.Product.Name;
                    var reference = p.Product.Reference;
                    var quantity = GetProductQuantity(p.Product.Identifier, productAdjustments);

                    if (existingProduct != null)
                    {
                        price = new ProductUnitPrice(existingProduct.PriceInfo.UnitPrice);
                        vat = existingProduct.PriceInfo.Vat;
                        name = new ProductName(existingProduct.Name);
                        reference = new ProductReference(existingProduct.Reference);

                        if (existingProduct.PriceInfo.Quantity.Value < Math.Abs(quantity.Value))
                            throw new InvalidOperationException("delivery.adjusted.product.invalid.quantity");
                    }

                    return DeliveryLine.CreateProductLine(p.Product.Identifier, reference, name, quantity, price, vat);
                }).ToList();
            
            adjustedProducts.AddRange(products
                .Where(p => p.Product.Returnable != null)
                .Select(p =>
                {
                    var existingReturnable =
                        delivery.Lines.SingleOrDefault(dl => dl.Identifier == p.Product.Returnable.Identifier.Value);

                    var price = p.Product.Returnable.UnitPrice;
                    var vat = p.Product.Returnable.Vat;
                    var name = p.Product.Returnable.Name;
                    var reference = p.Product.Returnable.Reference;
                    var quantity = GetProductQuantity(p.Product.Identifier, productAdjustments);

                    if (existingReturnable != null)
                    {
                        price = new ProductUnitPrice(existingReturnable.PriceInfo.UnitPrice);
                        vat = existingReturnable.PriceInfo.Vat;
                        name = new ReturnableName(existingReturnable.Name);
                        reference = new ReturnableReference(existingReturnable.Reference);

                        if (existingReturnable.PriceInfo.Quantity.Value < Math.Abs(quantity.Value))
                            throw new InvalidOperationException("delivery.adjusted.returnable.invalid.quantity");
                    }

                    return DeliveryLine.CreateReturnableLine(p.Product.Returnable.Identifier, reference, name, quantity, price, vat);
                }));

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