using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteDeliveryClosingsInputType : SheaftInputType<DeleteDeliveryClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteDeliveryClosingsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteDeliveryClosingsInput");
            
            descriptor
                .Field(c => c.ClosingIds)
                .Name("ids")
                .ID(nameof(DeliveryClosing));
        }
    }
}