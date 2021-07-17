using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BatchObservation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBatchObservationInputType : SheaftInputType<CreateBatchObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBatchObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateBatchObservationInput");
            
            descriptor
                .Field(c => c.BatchId)
                .ID(nameof(Batch))
                .Name("id");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
                
            descriptor
                .Field(c => c.VisibleToAll)
                .Name("visibleToAll");
        }
    }
}