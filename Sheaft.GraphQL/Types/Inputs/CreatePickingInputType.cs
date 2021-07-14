using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreatePickingInputType : SheaftInputType<CreatePickingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePickingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreatePickingInput");

            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.AutoStart)
                .Name("autostart");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .ID(nameof(PurchaseOrder))
                .Name("purchaseOrderIds");
        }
    }
}