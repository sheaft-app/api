using GreenDonut;
using HotChocolate.Types;
using Sheaft.Mediatr.Batch.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBatchInputType : SheaftInputType<CreateBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateBatchInput");
            
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