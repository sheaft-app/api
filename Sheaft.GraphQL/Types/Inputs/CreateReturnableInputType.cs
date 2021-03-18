using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateReturnableInputType : SheaftInputType<CreateReturnableDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateReturnableDto> descriptor)
        {
            descriptor.Name("CreateReturnableInput");
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.WholeSalePrice);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
