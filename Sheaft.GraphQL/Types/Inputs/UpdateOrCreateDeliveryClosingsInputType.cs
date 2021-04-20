using HotChocolate.Types;
using Sheaft.Mediatr.DeliveryClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateDeliveryClosingsInputType : SheaftInputType<UpdateOrCreateDeliveryClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateDeliveryClosingsCommand> descriptor)
        {
            descriptor.Name("UpdateOrCreateDeliveryClosingsInput");
            descriptor.Field(c => c.DeliveryId)
                .Name("id")
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<UpdateOrCreateClosingInputType>>>();
        }
    }
}