using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ExportPickingOrdersInputType : SheaftInputType<ExportPickingOrdersDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ExportPickingOrdersDto> descriptor)
        {
            descriptor.Name("ExportPickiginOrdersInput");
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.ExpectedDeliveryDate);

            descriptor.Field(c => c.PurchaseOrderIds)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}
