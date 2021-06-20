using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.DeliveryBatchs;
using Sheaft.GraphQL.PurchaseOrderDeliveries;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryBatchType : SheaftOutputType<DeliveryBatch>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryBatch> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<DeliveryBatchesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
            
            descriptor
                .Field(c => c.StartedOn)
                .Name("startedOn");
            
            descriptor
                .Field(c => c.CompletedOn)
                .Name("completedOn");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
            
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
            
            descriptor
                .Field(c => c.ScheduledOn)
                .Name("scheduledOn");
            
            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");
            
            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");
            
            descriptor
                .Field(c => c.DeliveriesCount)
                .Name("deliveriesCount");
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.Reason)
                .Name("reason");
            
            descriptor
                .Field("assignedTo")
                .ResolveWith<DeliveryBatchResolvers>(c => c.GetAssignedTo(default, default, default))
                .Type<UserType>();
            
            descriptor
                .Field(c => c.Deliveries)
                .Name("deliveries")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryBatchResolvers>(c => c.GetDeliveries(default, default, default, default))
                .Type<ListType<DeliveryType>>();
        }

        private class DeliveryBatchResolvers
        {
            public async Task<IEnumerable<Delivery>> GetDeliveries(DeliveryBatch deliveryBatch,
                [ScopedService] QueryDbContext context,
                DeliveriesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
            {
                var productsId = await context.Deliveries
                    .Where(p => p.DeliveryBatchId == deliveryBatch.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await deliveriesDataLoader.LoadAsync(productsId, token);
            }
            
            public Task<User> GetAssignedTo(DeliveryBatch deliveryBatch,
                UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                return usersDataLoader.LoadAsync(deliveryBatch.AssignedToId, token);
            }
        }
    }
}