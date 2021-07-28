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
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Deliveries;
using Sheaft.GraphQL.Observations;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Recalls;
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
                .ResolveWith<BatchResolvers>(c =>
                    c.GetObservations(default, default, default, default, default, default))
                .Type<ListType<ObservationType>>();

            descriptor
                .Field("observationsCount")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c =>
                    c.GetObservationsCount(default, default, default, default, default))
                .Type<IntType>();

            descriptor
                .Field("purchaseOrders")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c =>
                    c.GetPurchaseOrders(default, default, default, default, default, default))
                .Type<ListType<PurchaseOrderType>>();

            descriptor
                .Field("purchaseOrdersCount")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c =>
                    c.GetPurchaseOrdersCount(default, default, default, default, default))
                .Type<IntType>();

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
                .Field("deliveriesCount")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetDeliveriesCount(default, default, default))
                .Type<IntType>();

            descriptor
                .Field("clients")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetClients(default, default, default, default))
                .Type<ListType<UserType>>();

            descriptor
                .Field("clientsCount")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetClientsCount(default, default, default))
                .Type<IntType>();

            descriptor
                .Field("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c => c.GetProducts(default, default, default, default, default, default))
                .Type<ListType<ProductType>>();

            descriptor
                .Field("productsCount")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c =>
                    c.GetProductsCount(default, default, default, default, default))
                .Type<IntType>();

            descriptor
                .Field("recalls")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c =>
                    c.GetRecalls(default!, default!, default!, default))
                .Type<ListType<RecallType>>();

            descriptor
                .Field("recallsCount")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchResolvers>(c =>
                    c.GetRecallsCount(default!, default!, default!))
                .Type<IntType>();
        }

        private class BatchResolvers
        {
            public async Task<IEnumerable<Recall>> GetRecalls(Batch batch,
                [ScopedService] QueryDbContext context,
                RecallsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var recallIds = await context.Recalls
                    .Where(c => c.Batches.Any(p => p.BatchId == batch.Id) && (int) c.Status >= (int) RecallStatus.Ready)
                    .Select(a => a.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(recallIds, token);
            }

            public async Task<int> GetRecallsCount(Batch batch,
                [ScopedService] QueryDbContext context,
                CancellationToken token)
            {
                return await context.Recalls
                    .Where(c => c.Batches.Any(p => p.BatchId == batch.Id) && (int) c.Status >= (int) RecallStatus.Ready)
                    .CountAsync(token);
            }

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
                        .Where(cp =>
                            cp.ProducerId == currentUser.Data.Id && !cp.ReplyToId.HasValue &&
                            cp.Batches.Any(b => b.BatchId == batch.Id))
                        .Select(cp => cp.Id)
                        .ToListAsync(token);
                }
                else
                {
                    observationIds = await context.Observations
                        .Where(cp =>
                            (cp.VisibleToAll || cp.UserId == currentUser.Data.Id) && !cp.ReplyToId.HasValue &&
                            cp.Batches.Any(b => b.BatchId == batch.Id))
                        .Select(cp => cp.Id)
                        .ToListAsync(token);
                }

                var result = await dataLoader.LoadAsync(observationIds.Distinct().ToList(), token);
                return result.OrderBy(o => o.CreatedOn);
            }

            public async Task<int> GetObservationsCount(Batch batch,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return await context.Observations
                        .Where(cp =>
                            cp.VisibleToAll && !cp.ReplyToId.HasValue &&
                            cp.Batches.Any(b => b.BatchId == batch.Id))
                        .CountAsync(token);

                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                    return await context.Observations
                        .Where(cp =>
                            cp.ProducerId == currentUser.Data.Id && !cp.ReplyToId.HasValue &&
                            cp.Batches.Any(b => b.BatchId == batch.Id))
                        .CountAsync(token);

                return await context.Observations
                    .Where(cp =>
                        (cp.VisibleToAll || cp.UserId == currentUser.Data.Id) && !cp.ReplyToId.HasValue &&
                        cp.Batches.Any(b => b.BatchId == batch.Id))
                    .CountAsync(token);
            }

            public async Task<IEnumerable<Delivery>> GetDeliveries(Batch batch, [ScopedService] QueryDbContext context,
                DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var deliveryIds = await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.Picking.PreparedProducts.Any(pp =>
                        pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(deliveryIds.Distinct().ToList(), token);
            }

            public async Task<int> GetDeliveriesCount(Batch batch, [ScopedService] QueryDbContext context,
                CancellationToken token)
            {
                return await context.Deliveries
                    .Where(cp => cp.PurchaseOrders.Any(po => po.Picking.PreparedProducts.Any(pp =>
                        pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id))))
                    .CountAsync(token);
            }

            public async Task<IEnumerable<Product>> GetProducts(Batch batch,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                ProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return null;

                var productIds = new List<Guid>();
                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                {
                    productIds = await context.PurchaseOrders
                        .Where(p => p.ProducerId == currentUser.Data.Id)
                        .SelectMany(po => po.Picking.PreparedProducts
                            .Where(pp => pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id))
                            .Select(pp => pp.ProductId))
                        .ToListAsync(token);
                }
                else
                {
                    productIds = await context.PurchaseOrders
                        .Where(p => p.ClientId == currentUser.Data.Id)
                        .SelectMany(po => po.Picking.PreparedProducts
                            .Where(pp => pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id))
                            .Select(pp => pp.ProductId))
                        .ToListAsync(token);
                }

                return await dataLoader.LoadAsync(productIds.Distinct().ToList(), token);
            }

            public async Task<int> GetProductsCount(Batch batch,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return 0;

                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                    return await context.PurchaseOrders
                        .Where(p => p.ProducerId == currentUser.Data.Id)
                        .SelectMany(po => po.Picking.PreparedProducts
                            .Where(pp => pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id))
                            .Select(pp => pp.ProductId))
                        .Distinct()
                        .CountAsync(token);

                return await context.PurchaseOrders
                    .Where(p => p.ClientId == currentUser.Data.Id)
                    .SelectMany(po => po.Picking.PreparedProducts
                        .Where(pp => pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id))
                        .Select(pp => pp.ProductId))
                    .Distinct()
                    .CountAsync(token);
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
                        .SelectMany(cp =>
                            cp.PurchaseOrders.Where(po =>
                                    po.ProducerId == currentUser.Data.Id && po.Picking.PreparedProducts.Any(pp =>
                                        pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id)))
                                .Select(po => po.Id))
                        .ToListAsync(token);
                }
                else
                {
                    purchaseOrderIds = await context.PurchaseOrders
                        .Where(po => po.ClientId == currentUser.Data.Id && po.Picking.PreparedProducts.Any(pp =>
                            pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id)))
                        .Select(po => po.Id)
                        .ToListAsync(token);
                }

                return await dataLoader.LoadAsync(purchaseOrderIds.Distinct().ToList(), token);
            }

            public async Task<int> GetPurchaseOrdersCount(Batch batch,
                [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService,
                [Service] IOptionsSnapshot<RoleOptions> roleOptions,
                CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return 0;

                if (currentUser.Data.IsInRole(roleOptions.Value.Producer.Value))
                    return await context.Deliveries
                        .SelectMany(cp =>
                            cp.PurchaseOrders.Where(po =>
                                    po.ProducerId == currentUser.Data.Id && po.Picking.PreparedProducts.Any(pp =>
                                        pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id)))
                                .Select(po => po.Id))
                        .Distinct()
                        .CountAsync(token);
                return await context.PurchaseOrders
                    .Where(po => po.ClientId == currentUser.Data.Id && po.Picking.PreparedProducts.Any(pp =>
                        pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id)))
                    .Select(po => po.Id)
                    .Distinct()
                    .CountAsync(token);
            }

            public async Task<IEnumerable<User>> GetClients(Batch batch, [ScopedService] QueryDbContext context,
                UsersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var clientIds = await context.Deliveries
                    .SelectMany(cp =>
                        cp.PurchaseOrders.Where(po => po.Picking.PreparedProducts.Any(pp =>
                                pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id)))
                            .Select(po => po.ClientId))
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(clientIds.Distinct().ToList(), token);
            }

            public async Task<int> GetClientsCount(Batch batch, [ScopedService] QueryDbContext context, CancellationToken token)
            {
                return await context.Deliveries
                    .SelectMany(cp =>
                        cp.PurchaseOrders.Where(po => po.Picking.PreparedProducts.Any(pp =>
                                pp.PurchaseOrderId == po.Id && pp.Batches.Any(b => b.BatchId == batch.Id)))
                            .Select(po => po.ClientId))
                    .Distinct()
                    .CountAsync(token);
            }

            public Task<BatchDefinition> GetDefinition(Batch batch,
                BatchDefinitionsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(batch.DefinitionId, token);
            }
        }
    }
}