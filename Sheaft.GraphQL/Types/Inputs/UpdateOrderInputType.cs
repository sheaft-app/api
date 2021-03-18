using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrderInputType : SheaftInputType<UpdateOrderDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrderDto> descriptor)
        {
            descriptor.Name("UpdateOrderInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
