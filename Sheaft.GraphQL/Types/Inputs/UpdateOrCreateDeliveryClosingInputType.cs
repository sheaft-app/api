using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateDeliveryClosingInputType : SheaftInputType<UpdateOrCreateDeliveryClosingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateDeliveryClosingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateOrCreateDeliveryClosingInput");
            
            descriptor
                .Field(c => c.DeliveryId)
                .Name("id")
                .ID(nameof(DeliveryMode));
            
            descriptor
                .Field(c => c.Closing)
                .Name("closing")
                .Type<NonNullType<DeliveryClosingInputType>>();
        }
    }
}