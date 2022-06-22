using System.Diagnostics;
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
                .Where(o => o.DeliveryId == delivery.Id)
                .ToListAsync(token);

            var orderedProducts = orders
                .SelectMany(o => o.Lines.Where(l => l.LineKind == OrderLineKind.Product));

            var orderedReturnables = orders
                .SelectMany(o => o.Lines.Where(l => l.LineKind == OrderLineKind.Returnable));

            var productIdentifiers = productLines
                .Select(p => p.ProductIdentifier)
                .ToList();

            var products = await _context
                .Set<Product>()
                .Where(r =>
                    r.SupplierId == delivery.SupplierId
                    && productIdentifiers.Contains(r.Id))
                .Include(p => p.Returnable)
                .ToListAsync(token);

            if (products.Count != productIdentifiers.Count)
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.NotFound,
                    "delivery.lines.products.not.found");

            var batchIdentifiers = productLines
                .SelectMany(op => op.BatchIdentifiers)
                .Distinct()
                .ToList();

            if (batchIdentifiers.Any())
            {
                var batches = await _context
                    .Set<Batch>()
                    .Where(b => b.SupplierId == delivery.SupplierId)
                    .ToListAsync(token);

                if (batchIdentifiers.Count != batches.Count)
                    return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.NotFound,
                        "delivery.lines.batches.not.found");
            }

            var deliveryProductLines = products
                .Select(p =>
                    DeliveryLine.CreateProductLine(
                        p.Id, 
                        p.Reference, 
                        p.Name,
                        GetProductQuantity(p.Id, productLines),
                        GetEntityPrice(p.Id.Value, orderedProducts),
                        p.Vat,
                        GetProductOrder(p.Id, productLines),
                        GetProductBatches(p.Id, productLines)))
                .ToList();

            var deliveryReturnableLines = products
                .Where(p => p.Returnable != null)
                .Select(p =>
                    DeliveryLine.CreateReturnableLine(
                        p.Returnable.Id, 
                        p.Returnable.Reference,
                        p.Returnable.Name,
                        GetProductQuantity(p.Id, productLines),
                        GetEntityPrice(p.Returnable.Id.Value, orderedReturnables),
                        p.Vat,
                        GetProductOrder(p.Id, productLines)))
                .ToList();

            var deliveryLines = new List<DeliveryLine>();
            deliveryLines.AddRange(deliveryProductLines.Where(dp => dp.Quantity.Value > 0));
            deliveryLines.AddRange(deliveryReturnableLines.Where(dp => dp.Quantity.Value > 0));

            return Result.Success(deliveryLines.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.Unexpected, "database.error", e.Message);
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

    private ProductUnitPrice GetEntityPrice(string identifier, IEnumerable<OrderLine> orderLines)
    {
        var productPrice = orderLines.Single(p => p.Identifier == identifier).PriceInfo.UnitPrice;
        return new ProductUnitPrice(productPrice.Value);
    }
}