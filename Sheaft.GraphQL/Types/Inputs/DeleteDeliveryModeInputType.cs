using HotChocolate.Types;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteDeliveryModeInputType : SheaftInputType<DeleteDeliveryModeCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteDeliveryModeCommand> descriptor)
        {
            descriptor.Name("DeleteDeliveryModeInput");
            descriptor.Field(c => c.DeliveryModeId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}