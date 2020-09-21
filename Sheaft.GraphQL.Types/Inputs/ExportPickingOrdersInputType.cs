using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ExportPickingOrdersInputType : SheaftInputType<ExportPickingOrdersInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ExportPickingOrdersInput> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.ExpectedDeliveryDate);

            descriptor.Field(c => c.PurchaseOrderIds)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}
