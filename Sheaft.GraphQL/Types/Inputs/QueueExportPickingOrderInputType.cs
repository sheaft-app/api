using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.PickingOrders.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportPickingOrderInputType : SheaftInputType<QueueExportPickingOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportPickingOrderCommand> descriptor)
        {
            descriptor.Name("ExportPickingOrdersInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.PurchaseOrderIds)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}
