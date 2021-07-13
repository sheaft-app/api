using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.Extensions;
using Sheaft.GraphQL.Deliveries;
using Sheaft.GraphQL.Pickings;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PickingType : SheaftOutputType<Picking>
    {
        protected override void Configure(IObjectTypeDescriptor<Picking> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("Picking");
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<PickingsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
                
            descriptor
                .Field(c => c.Producer)
                .Name("producer");
            
            descriptor
                .Field(c => c.ProducerId)
                .ID(nameof(User))
                .Name("producerId");
            
            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");
            
            descriptor
                .Field(c => c.PickingFormUrl)
                .Name("preparationUrl");
            
            descriptor
                .Field(c => c.PreparedProductsCount)
                .Name("productsPreparedCount");
            
            descriptor
                .Field(c => c.ProductsToPrepareCount)
                .Name("productsToPrepareCount");
            
            descriptor
                .Field(c => c.CompletedOn)
                .Name("completedOn");
            
            descriptor
                .Field(c => c.StartedOn)
                .Name("startedOn");
            
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field("purchaseOrders")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<PickingResolvers>(c => c.GetPurchaseOrders(default, default, default, default))
                .Type<ListType<PurchaseOrderType>>();

            descriptor
                .Field(c => c.PreparedProducts)
                .Name("preparedProducts")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<PickingResolvers>(c => c.GetPreparedProducts(default, default, default, default))
                .Type<ListType<PreparedProductType>>();
            
            descriptor
                .Field(c => c.ProductsToPrepare)
                .Name("productsToPrepare")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<PickingResolvers>(c => c.GetProductsToPrepare(default, default, default, default))
                .Type<ListType<PickingProductType>>();
        }

        private class PickingResolvers
        {
            public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(Picking picking,
                [ScopedService] QueryDbContext context, 
                PurchaseOrdersByIdBatchDataLoader purchaseOrdersBatchDataLoader, CancellationToken token)
            {
                var purchaseOrderIds = await context.PurchaseOrders.Where(po => po.PickingId == picking.Id)
                    .Select(po => po.Id)
                    .ToListAsync(token);
                
                return await purchaseOrdersBatchDataLoader.LoadAsync(purchaseOrderIds, token);
            }

            public async Task<IEnumerable<PreparedProduct>> GetPreparedProducts(Picking picking,
                [ScopedService] QueryDbContext context,
                PreparedProductsByIdBatchDataLoader deliveryProductsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<PreparedProduct>()
                    .Where(p => p.PickingId == picking.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await deliveryProductsDataLoader.LoadAsync(productsId, token);
            }

            public async Task<IEnumerable<PickingProduct>> GetProductsToPrepare(Picking picking,
                [ScopedService] QueryDbContext context,
                PickingProductsByIdBatchDataLoader deliveryProductsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<PickingProduct>()
                    .Where(p => p.PickingId == picking.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await deliveryProductsDataLoader.LoadAsync(productsId, token);
            }
        }
    }
}
