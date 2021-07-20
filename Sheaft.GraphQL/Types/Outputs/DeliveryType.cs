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
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryType : SheaftOutputType<Delivery>
    {
        protected override void Configure(IObjectTypeDescriptor<Delivery> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("Delivery");
            
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
                .Field(c => c.BilledOn)
                .Name("billedOn");
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
            
            descriptor
                .Field("reference")
                .Resolve((ctx, token) => ctx.Parent<Delivery>().Reference.AsDeliveryIdentifier());
            
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
                .Field(c => c.ReturnablesCount)
                .Name("returnablesCount");
            
            descriptor
                .Field(c => c.DeliveryFormUrl)
                .Name("deliveryFormUrl");
            
            descriptor
                .Field(c => c.DeliveryReceiptUrl)
                .Name("deliveryReceiptUrl");
            
            descriptor
                .Field(c => c.ProductsToDeliverCount)
                .Name("productsToDeliverCount");
            
            descriptor
                .Field(c => c.ProductsDeliveredCount)
                .Name("productsDeliveredCount");
            
            descriptor
                .Field(c => c.BrokenProductsCount)
                .Name("brokenProductsCount");
            
            descriptor
                .Field(c => c.MissingProductsCount)
                .Name("missingProductsCount");
            
            descriptor
                .Field(c => c.ImproperProductsCount)
                .Name("improperProductsCount");
            
            descriptor
                .Field(c => c.ExcessProductsCount)
                .Name("excessProductsCount");
            
            descriptor
                .Field(c => c.ReturnedReturnablesCount)
                .Name("returnedReturnablesCount");

            descriptor
                .Field("purchaseOrders")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetPurchaseOrders(default, default, default, default))
                .Type<ListType<PurchaseOrderType>>();

            descriptor
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<DeliveryProductType>>();

            descriptor
                .Field(c => c.ReturnedReturnables)
                .Name("returnedReturnables")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetReturnedReturnables(default, default, default, default))
                .Type<ListType<DeliveryReturnableType>>();
            
            descriptor
                .Field(c => c.Client)
                .ResolveWith<DeliveryResolvers>(c => c.GetClient(default, default, default))
                .Name("client")
                .Type<UserType>();
            
            descriptor
                .Field(c => c.Producer)
                .ResolveWith<DeliveryResolvers>(c => c.GetProducer(default, default, default))
                .Name("producer")
                .Type<ProducerType>();
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

            public async Task<IEnumerable<DeliveryProduct>> GetProducts(Delivery delivery,
                [ScopedService] QueryDbContext context,
                DeliveryProductsByIdBatchDataLoader deliveryProductsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<DeliveryProduct>()
                    .Where(p => p.DeliveryId == delivery.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await deliveryProductsDataLoader.LoadAsync(productsId.Distinct().ToList(), token);
            }

            public async Task<IEnumerable<DeliveryReturnable>> GetReturnedReturnables(Delivery delivery,
                [ScopedService] QueryDbContext context,
                DeliveryReturnablesByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var returnableIds = await context.Set<DeliveryReturnable>()
                    .Where(p => p.DeliveryId == delivery.Id)
                    .Select(p => p.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(returnableIds, token);
            }

            public Task<Producer> GetProducer(Delivery delivery,
                ProducersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(delivery.ProducerId, token);
            }

            public Task<User> GetClient(Delivery delivery,
                UsersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(delivery.ClientId, token);
            }
        }
    }
}
