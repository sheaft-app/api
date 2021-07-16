using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BatchObservation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteBatchObservationInputType : SheaftInputType<DeleteBatchObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteBatchObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteBatchObservationInput");
            
            descriptor
                .Field(c => c.BatchObservationId)
                .ID(nameof(BatchObservation))
                .Name("id");
        }
    }
}