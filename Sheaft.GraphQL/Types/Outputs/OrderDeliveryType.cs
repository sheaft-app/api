using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.DeliveryModes;
using Sheaft.GraphQL.Orders;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderDeliveryType : SheaftOutputType<OrderDelivery>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderDelivery> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
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
                .Field("expectedDelivery")
                .ResolveWith<OrderDeliveryResolvers>(c => c.GetExpectedDelivery(default))
                .Type<NonNullType<ExpectedDeliveryDtoType>>();
        }

        private class OrderDeliveryResolvers
        {
            public Task<DeliveryMode> GetDeliveryMode(OrderDelivery orderDelivery,
                DeliveryModesByIdBatchDataLoader deliveryModesDataLoader, CancellationToken token)
            {
                return deliveryModesDataLoader.LoadAsync(orderDelivery.DeliveryModeId, token);
            }
            
            public Task<ExpectedDeliveryDto> GetExpectedDelivery(OrderDelivery orderDelivery)
            {
                return Task.FromResult(new ExpectedDeliveryDto()
                {
                    From = orderDelivery.From,
                    To = orderDelivery.To,
                    Day = orderDelivery.Day,
                    ExpectedDeliveryDate = orderDelivery.ExpectedDeliveryDate,
                });
            }
        }
    }
}