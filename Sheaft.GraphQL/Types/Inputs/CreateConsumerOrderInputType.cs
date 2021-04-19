using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateConsumerOrderInputType : SheaftInputType<CreateConsumerOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateConsumerOrderCommand> descriptor)
        {
            descriptor.Name("CreateOrderInput");
            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
