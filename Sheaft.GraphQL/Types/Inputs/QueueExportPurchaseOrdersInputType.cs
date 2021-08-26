using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportPurchaseOrdersInputType : SheaftInputType<QueueExportPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportPurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ExportPurchaseOrdersInput");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
        }
    }
}