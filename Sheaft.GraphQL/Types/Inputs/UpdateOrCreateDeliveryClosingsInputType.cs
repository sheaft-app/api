using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateDeliveryClosingsInputType : SheaftInputType<UpdateOrCreateDeliveryClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateDeliveryClosingsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateOrCreateDeliveryClosingsInput");
            
            descriptor
                .Field(c => c.DeliveryId)
                .Name("id")
                .ID(nameof(DeliveryMode));
            
            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .Type<NonNullType<ListType<DeliveryClosingInputType>>>();
        }
    }
}