using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Transaction.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportTransactionsInputType : SheaftInputType<QueueExportTransactionsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportTransactionsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ExportTransactionsInput");
            
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