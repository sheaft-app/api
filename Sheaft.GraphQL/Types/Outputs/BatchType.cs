using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Deliveries;
using Sheaft.GraphQL.Observations;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

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
                .Field("observations")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetObservations(default, default, default, default, default, default))
                .Type<ListType<ObservationType>>();
            
            descriptor
                .Field("purchaseOrders")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetPurchaseOrders(default, default, default, default, default, default))
                .Type<ListType<PurchaseOrderType>>();
            
            descriptor
                .Field(c => c.Fields)
                .Authorize(Policies.PRODUCER)
                .Type<ListType<BatchFieldType>>()
                .Name("fields");
            
            descriptor
                .Field(c => c.Definition)
                .Authorize(Policies.PRODUCER)
                .Name("definition")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetDefinition(default, default, default))
                .Type<BatchDefinitionType>();
            
            descriptor
                .Field("deliveries")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetDeliveries(default, default, default, default))
                .Type<ListType<DeliveryType>>();
            
            descriptor
                .Field("clients")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetClients(default, default, default, default))
                .Type<ListType<UserType>>();
            
            descriptor
                .Field("products")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<ProductType>>();
        }

        private class BatchResolvers
        {
            public async Task<IEnumerable<Observation>> GetObservations(Batch batch,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                ObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                var observationIds = new List<Guid>();
                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                {
                    observationIds = await context.Observations
                            .Where(cp => cp.ProducerId == currentUser.Data.Id && !cp.ReplyToId.HasValue && cp.VisibleToAll && cp.Batches.Any(b => b.BatchId == batch.Id))
                            .Select(cp => cp.Id)
                            .ToListAsync(token);
                }
                else
                {
                    observationIds = await context.Observations
                    .Where(cp => cp.UserId == currentUser.Data.Id && !cp.ReplyToId.HasValue && cp.Batches.Any(b => b.BatchId == batch.Id))
                    .Select(cp => cp.Id)
                    .ToListAsync(token);
                }

                var result = await dataLoader.LoadAsync(observationIds, token);
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
            
            public async Task<IEnumerable<Product>> GetProducts(Batch batch, [ScopedService] QueryDbContext context,
                ProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var productIds = await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.Picking.PreparedProducts.Any(pp => pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .SelectMany(cp => cp.PurchaseOrders.SelectMany(po => po.Picking.PreparedProducts.Select(pp => pp.ProductId)))
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(productIds, token);
            }
            
            public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(Batch batch,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                PurchaseOrdersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                var purchaseOrderIds = new List<Guid>();
                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                {
                    purchaseOrderIds = await context.Deliveries
                       .Where(cp => cp.PurchaseOrders.Any(po => po.ProducerId == currentUser.Data.Id && po.Picking.PreparedProducts.Any(pp => pp.Batches.Any(b => b.BatchId == batch.Id))))
                       .SelectMany(cp => cp.PurchaseOrders.Select(po => po.Id))
                       .ToListAsync(token);
                }
                else
                {
                    purchaseOrderIds = await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.ClientId == currentUser.Data.Id && po.Picking.PreparedProducts.Any(pp => pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .SelectMany(cp => cp.PurchaseOrders.Select(po => po.Id))
                    .ToListAsync(token);
                }

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