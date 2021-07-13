using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class StartPickingInputType : SheaftInputType<StartPickingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<StartPickingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("StartPickingInput");

            descriptor
                .Field(c => c.PickingId)
                .ID(nameof(Picking))
                .Name("id");
        }
    }
}