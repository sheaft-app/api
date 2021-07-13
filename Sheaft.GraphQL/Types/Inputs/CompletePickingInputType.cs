using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CompletePickingInputType : SheaftInputType<CompletePickingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CompletePickingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CompletePickingInput");

            descriptor
                .Field(c => c.PickingId)
                .ID(nameof(Picking))
                .Name("id");
        }
    }
}