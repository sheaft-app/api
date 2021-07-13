using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeletePickingInputType : SheaftInputType<DeletePickingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeletePickingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeletePickingInput");

            descriptor
                .Field(c => c.PickingId)
                .ID(nameof(Picking))
                .Name("id");
        }
    }
}