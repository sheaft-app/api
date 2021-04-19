using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportPurchaseOrdersInputType : SheaftInputType<QueueExportPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("ExportPurchaseOrdersInput");
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}