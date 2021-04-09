using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateWebPayinInputType : SheaftInputType<CreateWebPayinDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateWebPayinDto> descriptor)
        {
            descriptor.Name("PayOrderInput");
            
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}