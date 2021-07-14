using HotChocolate.Types;
using Sheaft.Mediatr.Batch.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateBatchInputType : SheaftInputType<UpdateBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateBatchInput");
            
            descriptor
                .Field(c => c.BatchId)
                .ID(nameof(Domain.Batch))
                .Name("id");
            
            descriptor
                .Field(c => c.Number)
                .Name("number");

            descriptor
                .Field(c => c.DLC)
                .Name("dlc");
            
            descriptor
                .Field(c => c.DLUO)
                .Name("dluo");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
        }
    }
}