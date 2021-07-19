using HotChocolate.Types;
using Sheaft.Mediatr.Accounting.Commands;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportAccountingInputType : SheaftInputType<QueueExportAccountingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportAccountingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("QueueExportAccountingInput");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
                
            descriptor
                .Field(c => c.Kinds)
                .Name("kinds");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
        }
    }
}