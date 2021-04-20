using HotChocolate.Types;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBusinessOrderInputType : SheaftInputType<CreateBusinessOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBusinessOrderCommand> descriptor)
        {
            descriptor.Name("CreatePurchaseOrderInput");
            descriptor.Field(c => c.UserId)
                .Name("id");

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}