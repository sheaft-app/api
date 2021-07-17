using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Observation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ReplyToObservationInputType : SheaftInputType<ReplyToObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ReplyToObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ReplyToObservationInput");
            
            descriptor
                .Field(c => c.ObservationId)
                .ID(nameof(Observation))
                .Name("id");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
        }
    }
}