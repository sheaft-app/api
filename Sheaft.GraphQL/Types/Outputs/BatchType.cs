using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Deliveries;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BatchType : SheaftOutputType<Batch>
    {
        protected override void Configure(IObjectTypeDescriptor<Batch> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<BatchesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Number)
                .Name("number");
            
            descriptor
                .Field(c => c.DLC)
                .Name("dlc");
            
            descriptor
                .Field(c => c.DDM)
                .Name("ddm");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
            
            descriptor
                .Field(c => c.Observations)
                .Name("observations")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetObservations(default, default, default, default))
                .Type<ListType<BatchObservationType>>();
            
            descriptor
                .Field(c => c.Fields)
                .Type<ListType<BatchFieldType>>()
                .Name("fields");
            
            descriptor
                .Field(c=> c.Definition)
                .Name("definition")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetDefinition(default, default, default))
                .Type<BatchDefinitionType>();
            
            descriptor
                .Field("deliveries")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetDeliveries(default, default, default, default))
                .Type<ListType<DeliveryType>>();
            
            descriptor
                .Field("clients")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetClients(default, default, default, default))
                .Type<ListType<UserType>>();
            
            descriptor
                .Field("purchaseOrders")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetPurchaseOrders(default, default, default, default))
                .Type<ListType<PurchaseOrderType>>();
        }

        private class BatchResolvers
        {
            public async Task<IEnumerable<BatchObservation>> GetObservations(Batch batch, [ScopedService] QueryDbContext context,
                BatchObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var batchObservationIds = await context.Set<BatchObservation>()
                    .Where(cp => cp.BatchId == batch.Id && !cp.ReplyToId.HasValue)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                var result = await dataLoader.LoadAsync(batchObservationIds, token);
                return result.OrderBy(o => o.CreatedOn);
            }
            
            public async Task<IEnumerable<Delivery>> GetDeliveries(Batch batch, [ScopedService] QueryDbContext context,
                DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var deliveryIds = await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.Picking.PreparedProducts.Any(pp => pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(deliveryIds, token);
            }
            
            public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(Batch batch, [ScopedService] QueryDbContext context,
                PurchaseOrdersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var purchaseOrderIds = await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.Picking.PreparedProducts.Any(pp => pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .SelectMany(cp => cp.PurchaseOrders.Select(po => po.Id))
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(purchaseOrderIds.Distinct().ToList(), token);
            }
            
            public async Task<IEnumerable<User>> GetClients(Batch batch, [ScopedService] QueryDbContext context,
                UsersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var clientIds = await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.Picking.PreparedProducts.Any(pp => pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .SelectMany(cp => cp.PurchaseOrders.Select(po => po.ClientId))
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(clientIds.Distinct().ToList(), token);
            }
            
            public Task<BatchDefinition> GetDefinition(Batch batch, 
                BatchDefinitionsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(batch.DefinitionId, token);
            }
        }
    }
}