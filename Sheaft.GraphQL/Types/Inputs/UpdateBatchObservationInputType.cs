using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BatchObservation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateBatchObservationInputType : SheaftInputType<UpdateBatchObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBatchObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateBatchObservationInput");
            
            descriptor
                .Field(c => c.BatchObservationId)
                .ID(nameof(BatchObservation))
                .Name("id");
            
            descriptor
                .Field(c => c.Comment)
                .Name("name");
        }
    }
}