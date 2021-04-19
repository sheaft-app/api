using HotChocolate.Types;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResetOrderInputType : SheaftInputType<ResetOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResetOrderCommand> descriptor)
        {
            descriptor.Name("ResetOrderInput");
            
            descriptor.Field(c => c.OrderId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}