using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.DeliveryClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateDeliveryClosingInputType : SheaftInputType<UpdateOrCreateDeliveryClosingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateDeliveryClosingCommand> descriptor)
        {
            descriptor.Name("UpdateOrCreateDeliveryClosingInput");
            descriptor.Field(c => c.DeliveryId)
                .Name("id")
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closing)
                .Type<NonNullType<UpdateOrCreateClosingInputType>>();
        }
    }
}