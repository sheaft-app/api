using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.PickingOrders.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportPickingOrderInputType : SheaftInputType<QueueExportPickingOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportPickingOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ExportPickingOrdersInput");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");

            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));
        }
    }
}
