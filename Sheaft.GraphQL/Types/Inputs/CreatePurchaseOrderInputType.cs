using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreatePurchaseOrderInputType : SheaftInputType<CreatePurchaseOrderDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePurchaseOrderDto> descriptor)
        {
            descriptor.Name("CreatePurchaseOrderInput");
            descriptor.Field(c => c.Comment);
            descriptor.Field(c => c.ExpectedDeliveryDate);

            descriptor.Field(c => c.DeliveryModeId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.ProducerId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
