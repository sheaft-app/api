using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BatchObservation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ReplyToBatchObservationInputType : SheaftInputType<ReplyToBatchObservationCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ReplyToBatchObservationCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ReplyToBatchObservationInput");
            
            descriptor
                .Field(c => c.BatchObservationId)
                .ID(nameof(BatchObservation))
                .Name("id");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
        }
    }
}