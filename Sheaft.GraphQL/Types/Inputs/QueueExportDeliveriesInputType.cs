using HotChocolate.Types;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportDeliveriesInputType : SheaftInputType<QueueExportDeliveriesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportDeliveriesCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("QueueExportDeliveriesInput");
            
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