using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class RetrieveDeliveryBatches : IRetrieveDeliveryBatches
{
    private readonly IDbContext _context;

    public RetrieveDeliveryBatches(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<DeliveryBatch>>> Get(Delivery delivery, IEnumerable<ProductBatches>? productsBatches, CancellationToken token)
    {
        try
        {
            var batchIdentifiers = productsBatches?
                .SelectMany(pb => pb.BatchIdentifiers)
                .Distinct()
                .ToList() ?? new List<BatchId>();

            if (!batchIdentifiers.Any())
                return Result.Success(new List<DeliveryBatch>().AsEnumerable());

            var batches = await _context.Set<Batch>()
                .Where(b => batchIdentifiers.Contains(b.Identifier))
                .ToListAsync(token);

            if (batchIdentifiers.Count != batches.Count)
                return Result.Failure<IEnumerable<DeliveryBatch>>(ErrorKind.NotFound, "delivery.batches.not.found");

            var deliveryBatches = new List<DeliveryBatch>();
            foreach (var batchIdentifier in batchIdentifiers)
            {
                var products = productsBatches.Where(pb => pb.BatchIdentifiers.Contains(batchIdentifier));
                deliveryBatches.AddRange(products.Select(p => new DeliveryBatch(batchIdentifier, p.ProductIdentifier)));
            }
            
            return Result.Success(deliveryBatches.Distinct());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<DeliveryBatch>>(ErrorKind.Unexpected, "database.error");
        }
    }
}