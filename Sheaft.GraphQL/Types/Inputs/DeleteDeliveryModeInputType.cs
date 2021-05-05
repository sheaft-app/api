using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteDeliveryModeInputType : SheaftInputType<DeleteDeliveryModeCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteDeliveryModeCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteDeliveryModeInput");
            
            descriptor
                .Field(c => c.DeliveryModeId)
                .Name("id")
                .ID(nameof(DeliveryMode));
        }
    }
}