using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class MarkDeliveryAsBilledInputType : SheaftInputType<MarkDeliveryAsBilledCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MarkDeliveryAsBilledCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("MarkDeliveryAsBilledInput");

            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(Delivery))
                .Name("id");
        }
    }
}