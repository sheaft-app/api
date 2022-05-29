using System.Diagnostics;
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
                .SingleOrDefaultAsync(c => c.IsDefault && c.SupplierId == delivery.SupplierId, token);

            if (catalog == null)
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.NotFound, "catalog.not.found");
            
            var productsToAdjust = productAdjustments.Select(p => p.Identifier).ToList();
            var products = catalog.Products.Where(p => productsToAdjust.Contains(p.Product.Id))
                .ToList();
            
            if (products.Count != productAdjustments.Count())
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.BadRequest,
                    "delivery.products.to.adjust.not.found");

            var adjustedProducts = products.Select(p =>
                {
                    var existingProduct =
                        delivery.Lines.SingleOrDefault(dl => dl.Identifier == p.Product.Id.Value);

                    var price = p.UnitPrice;
                    var vat = p.Product.Vat;
                    var name = p.Product.Name;
                    var reference = p.Product.Reference;
                    var quantity = GetProductQuantity(p.Product.Id, productAdjustments);
                    var batches = GetProductBatches(p.Product.Id, delivery.Lines);
                    var order = GetProductOrder(p.Product.Id, delivery.Lines);

                    if (existingProduct != null)
                    {
                        price = new ProductUnitPrice(existingProduct.PriceInfo.UnitPrice);
                        vat = existingProduct.Vat;
                        name = new ProductName(existingProduct.Name);
                        reference = new ProductReference(existingProduct.Reference);

                        if (existingProduct.Quantity.Value < Math.Abs(quantity.Value))
                            throw new InvalidOperationException("delivery.adjusted.product.invalid.quantity");
                    }

                    return DeliveryLine.CreateProductLine(p.Product.Id, reference, name, quantity, price, vat, order, batches);
                }).ToList();
            
            adjustedProducts.AddRange(products
                .Where(p => p.Product.Returnable != null)
                .Select(p =>
                {
                    Debug.Assert(p.Product.Returnable != null, "p.Product.Returnable != null");
                    
                    var existingReturnable =
                        delivery.Lines.SingleOrDefault(dl => dl.Identifier == p.Product.Returnable.Id.Value);

                    var price = p.Product.Returnable.UnitPrice;
                    var vat = p.Product.Returnable.Vat;
                    var name = p.Product.Returnable.Name;
                    var reference = p.Product.Returnable.Reference;
                    var quantity = GetProductQuantity(p.Product.Id, productAdjustments);
                    var order = GetProductOrder(p.Product.Id, delivery.Lines);

                    if (existingReturnable != null)
                    {
                        price = new ProductUnitPrice(existingReturnable.PriceInfo.UnitPrice);
                        vat = existingReturnable.Vat;
                        name = new ReturnableName(existingReturnable.Name);
                        reference = new ReturnableReference(existingReturnable.Reference);

                        if (existingReturnable.Quantity.Value < Math.Abs(quantity.Value))
                            throw new InvalidOperationException("delivery.adjusted.returnable.invalid.quantity");
                    }

                    return DeliveryLine.CreateReturnableLine(p.Product.Returnable.Id, reference, name, quantity, price, vat, order);
                }));

            return Result.Success(adjustedProducts.AsEnumerable());
        }
        catch (InvalidOperationException e)
        {
            return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }

    private Quantity GetProductQuantity(ProductId productIdentifier, IEnumerable<ProductAdjustment> productAdjustments)
    {
        return productAdjustments.Single(p => p.Identifier == productIdentifier).Quantity;
    }

    private IEnumerable<BatchId> GetProductBatches(ProductId productIdentifier, IEnumerable<DeliveryLine> deliveryLines)
    {
        return deliveryLines.Single(p => p.Identifier == productIdentifier.Value).Batches?
            .Select(b => b.BatchIdentifier)
            .ToList() ?? new List<BatchId>();
    }

    private DeliveryOrder GetProductOrder(ProductId productIdentifier, IEnumerable<DeliveryLine> deliveryLines)
    {
        var order = deliveryLines.Single(p => p.Identifier == productIdentifier.Value).Order;
        if (order == null)
            throw new InvalidOperationException("deliveryline.order.null");
        
        return new DeliveryOrder(order.Reference, order.PublishedOn);
    }
}