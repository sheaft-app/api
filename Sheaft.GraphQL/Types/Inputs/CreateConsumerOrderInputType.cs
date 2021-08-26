using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateConsumerOrderInputType : SheaftInputType<CreateConsumerOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateConsumerOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateOrderInput");
            
            descriptor
                .Field(c => c.Donation)
                .Name("donation");
            
            descriptor
                .Field(c => c.UserId)
                .Name("userId")
                .ID(nameof(User));

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
