using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateConsumerOrderInputType : SheaftInputType<UpdateConsumerOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerOrderCommand> descriptor)
        {
            descriptor.Name("UpdateOrderInput");
            descriptor.Field(c => c.OrderId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
            
            descriptor.Field(c => c.UserId)
                .Name("UserId")
                .Type<IdType>();

            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
