using HotChocolate.Types;
using Sheaft.Mediatr.Billing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportTimeRangedBillingsInputType : SheaftInputType<QueueExportTimeRangedBillingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportTimeRangedBillingsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("QueueExportTimeRangedBillingsInput");
            
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