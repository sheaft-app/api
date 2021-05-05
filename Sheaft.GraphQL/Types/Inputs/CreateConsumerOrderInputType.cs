using HotChocolate.Types;
using Sheaft.Application.Models;
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
                .Field(c => c.ProducersExpectedDeliveries)
                .Name("producersExpectedDeliveries")
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor
                .Field(c => c.Products)
                .Name("products")
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
