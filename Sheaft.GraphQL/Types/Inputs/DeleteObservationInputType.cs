using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Observation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteObservationInputType : SheaftInputType<DeleteObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteObservationInput");
            
            descriptor
                .Field(c => c.BatchObservationId)
                .ID(nameof(Observation))
                .Name("id");
        }
    }
}