using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreatePurchaseOrderInputType : SheaftInputType<CreatePurchaseOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePurchaseOrderInput> descriptor)
        {
            descriptor.Field(c => c.Comment);
            descriptor.Field(c => c.ExpectedDeliveryDate);

            descriptor.Field(c => c.DeliveryModeId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.ProducerId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
}
