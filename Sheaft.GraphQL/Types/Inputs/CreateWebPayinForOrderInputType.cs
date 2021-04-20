using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.WebPayin.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateWebPayinForOrderInputType : SheaftInputType<CreateWebPayinForOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateWebPayinForOrderCommand> descriptor)
        {
            descriptor.Name("PayOrderInput");
            
            descriptor.Field(c => c.OrderId)
                .Name("id")
                .Type<NonNullType<IdType>>();
        }
    }
}