using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SkipDeliveryInputType : SheaftInputType<SkipDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SkipDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SkipDeliveryInput");

            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(Delivery))
                .Name("id");
        }
    }
}