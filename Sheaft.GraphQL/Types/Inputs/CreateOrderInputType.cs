using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateOrderInputType : SheaftInputType<CreateOrderDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateOrderDto> descriptor)
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
