using HotChocolate.Types;
using Sheaft.Mediatr.Batch.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteBatchInputType : SheaftInputType<DeleteBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteBatchInput");
            
            descriptor
                .Field(c => c.BatchId)
                .ID(nameof(Domain.Batch))
                .Name("id");
        }
    }
}