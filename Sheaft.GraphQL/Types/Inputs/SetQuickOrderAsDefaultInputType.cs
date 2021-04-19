using HotChocolate.Types;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetQuickOrderAsDefaultInputType : SheaftInputType<SetQuickOrderAsDefaultCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetQuickOrderAsDefaultCommand> descriptor)
        {
            descriptor.Name("SetQuickOrderAsDefaultInput");
            
            descriptor.Field(c => c.UserId)
                .Type<IdType>();
            
            descriptor.Field(c => c.QuickOrderId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}