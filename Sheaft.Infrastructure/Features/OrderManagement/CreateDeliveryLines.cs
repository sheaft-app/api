using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class CreateDeliveryLines : ICreateDeliveryLines
{
    private readonly IDbContext _context;

    public CreateDeliveryLines(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<DeliveryLine>>> Get(Delivery delivery,
        IEnumerable<DeliveryProductBatches> productLines, CancellationToken token)
    {
        try
        {
            var orders = await _context
                .Set<Order>()
                .Where(o => o.DeliveryIdentifier == delivery.Identifier)
                .ToListAsync(token);

            var orderedProducts = orders
                .SelectMany(o => o.Lines.Where(l => l.LineKind == OrderLineKind.Product));

            var productIdentifiers = productLines.Select(p => p.ProductIdentifier).ToList();
            var products = await _context
                .Set<Product>()
                .Where(r => r.SupplierIdentifier == delivery.SupplierIdentifier &&
                            productIdentifiers.Contains(r.Identifier))
                .ToListAsync(token);

            if (products.Count != productIdentifiers.Count)
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.NotFound,
                    "delivery.lines.products.not.found");

            var batchIdentifiers = productLines.SelectMany(op => op.BatchIdentifiers).Distinct().ToList();
            if (batchIdentifiers.Any())
            {
                var batches = await _context
                    .Set<Batch>()
                    .Where(b => b.SupplierIdentifier == delivery.SupplierIdentifier)
                    .ToListAsync(token);

                if (batchIdentifiers.Count != batches.Count)
                    return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.NotFound,
                        "delivery.lines.batches.not.found");
            }

            var deliveryProductLines = products.Select(p =>
                    DeliveryLine.CreateProductLine(p.Identifier, p.Reference, p.Name,
                        GetProductQuantity(p.Identifier, productLines), GetProductPrice(p.Identifier, orderedProducts),
                        p.Vat, GetProductOrder(p.Identifier, productLines),
                        GetProductBatches(p.Identifier, productLines)))
                .ToList();

            var deliveryReturnableLines = products.Where(p => p.Returnable != null).Select(p =>
                DeliveryLine.CreateReturnableLine(p.Returnable.Identifier, p.Returnable.Reference, p.Returnable.Name,
                    GetProductQuantity(p.Identifier, productLines), GetProductPrice(p.Identifier, orderedProducts),
                    p.Vat, GetProductOrder(p.Identifier, productLines))).ToList();

            var deliveryLines = new List<DeliveryLine>();
            deliveryLines.AddRange(deliveryProductLines.Where(dp => dp.Quantity.Value > 0));
            deliveryLines.AddRange(deliveryReturnableLines.Where(dp => dp.Quantity.Value > 0));

            return Result.Success(deliveryLines.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.Unexpected, "database.error");
        }
    }

    private IEnumerable<BatchId> GetProductBatches(ProductId productIdentifier,
        IEnumerable<DeliveryProductBatches> orderedProducts)
    {
        return orderedProducts
            .Single(p => p.ProductIdentifier == productIdentifier).BatchIdentifiers?
            .Distinct()
            .ToList() ?? new List<BatchId>();
    }

    private Quantity GetProductQuantity(ProductId productIdentifier,
        IEnumerable<DeliveryProductBatches> orderedProducts)
    {
        return orderedProducts.Single(p => p.ProductIdentifier == productIdentifier).Quantity;
    }

    private DeliveryOrder GetProductOrder(ProductId productIdentifier,
        IEnumerable<DeliveryProductBatches> orderedProducts)
    {
        var order = orderedProducts.Single(p => p.ProductIdentifier == productIdentifier).Order;
        return new DeliveryOrder(order.Reference, order.PublishedOn);
    }

    private ProductUnitPrice GetProductPrice(ProductId productIdentifier, IEnumerable<OrderLine> orderedProducts)
    {
        var productPrice = orderedProducts.Single(p => p.Identifier == productIdentifier.Value).PriceInfo.UnitPrice;
        return new ProductUnitPrice(productPrice.Value);
    }
}