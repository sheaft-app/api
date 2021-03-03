using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrderInputType : SheaftInputType<UpdateOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrderInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
}
