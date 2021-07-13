using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PausePickingInputType : SheaftInputType<PausePickingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PausePickingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PausePickingInput");

            descriptor
                .Field(c => c.PickingId)
                .ID(nameof(Picking))
                .Name("id");
        }
    }
}