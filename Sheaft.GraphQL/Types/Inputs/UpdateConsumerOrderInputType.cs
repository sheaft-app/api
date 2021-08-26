using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateConsumerOrderInputType : SheaftInputType<UpdateConsumerOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateOrderInput");
            
            descriptor
                .Field(c => c.OrderId)
                .Name("id")
                .ID(nameof(Order));
            
            descriptor
                .Field(c => c.UserId)
                .Name("userId")
                .ID(nameof(User));

            descriptor
                .Field(c => c.Donation)
                .Name("donation");

            descriptor
                .Field(c => c.ProducersExpectedDeliveries)
                .Name("deliveries")
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor
                .Field(c => c.Products)
                .Name("products")
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
