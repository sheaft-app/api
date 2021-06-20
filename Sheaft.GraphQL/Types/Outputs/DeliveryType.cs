using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.PurchaseOrderDeliveries;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryType : SheaftOutputType<Delivery>
    {
        protected override void Configure(IObjectTypeDescriptor<Delivery> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DeliveriesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.ScheduledOn)
                .Name("scheduledOn");
                
            descriptor
                .Field(c => c.DeliveredOn)
                .Name("deliveredOn");
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
            
            descriptor
                .Field(c => c.Client)
                .Name("client");
            
            descriptor
                .Field(c => c.ClientId)
                .ID(nameof(User))
                .Name("clientId");

            descriptor
                .Field(c => c.Address)
                .Name("address");

            descriptor
                .Field(c => c.Position)
                .Name("position");

            descriptor
                .Field(c => c.ReceptionedBy)
                .Name("receptionedBy");

            descriptor
                .Field(c => c.Comment)
                .Name("comment");

            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");

            descriptor
                .Field("purchaseOrders")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetPurchaseOrders(default, default, default, default))
                .Type<ListType<PurchaseOrderType>>();
        }

        private class DeliveryResolvers
        {
            public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(Delivery delivery,
                [ScopedService] QueryDbContext context, 
                PurchaseOrdersByIdBatchDataLoader purchaseOrdersBatchDataLoader, CancellationToken token)
            {
                var purchaseOrderIds = await context.PurchaseOrders.Where(po => po.DeliveryId == delivery.Id)
                    .Select(po => po.Id)
                    .ToListAsync(token);
                
                return await purchaseOrdersBatchDataLoader.LoadAsync(purchaseOrderIds, token);
            }
        }
    }
}
