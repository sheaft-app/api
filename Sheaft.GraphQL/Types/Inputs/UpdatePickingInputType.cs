using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdatePickingInputType : SheaftInputType<UpdatePickingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePickingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdatePickingInput");

            descriptor
                .Field(c => c.PickingId)
                .ID(nameof(Picking))
                .Name("id");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .ID(nameof(PurchaseOrder))
                .Name("purchaseOrderIds");
        }
    }
}