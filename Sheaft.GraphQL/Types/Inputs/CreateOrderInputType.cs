using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateOrderInputType : SheaftInputType<CreateOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateOrderInput> descriptor)
        {
            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
}
