using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.DeliveryModes;
using Sheaft.GraphQL.Orders;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderDeliveryType : SheaftOutputType<OrderDelivery>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderDelivery> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.DeliveryModeId)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<OrderDeliveriesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");

            descriptor
                .Field(c => c.DeliveryMode)
                .Name("deliveryMode")
                .ResolveWith<OrderDeliveryResolvers>(c => c.GetDeliveryMode(default, default, default))
                .Type<NonNullType<DeliveryModeType>>();

            descriptor
                .Field(c => c.ExpectedDelivery)
                .Name("expectedDelivery")
                .Type<NonNullType<ExpectedOrderDeliveryType>>();
        }

        private class OrderDeliveryResolvers
        {
            public Task<DeliveryMode> GetDeliveryMode(OrderDelivery orderDelivery,
                DeliveryModesByIdBatchDataLoader deliveryModesDataLoader, CancellationToken token)
            {
                return deliveryModesDataLoader.LoadAsync(orderDelivery.DeliveryModeId, token);
            }
        }
    }
}