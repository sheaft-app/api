using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PayOrderInputType : SheaftInputType<PayOrderDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PayOrderDto> descriptor)
        {
            descriptor.Name("PayOrderInput");
            descriptor.Field(c => c.CardIdentifier);
            
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}