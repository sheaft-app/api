using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateReturnableInputType : SheaftInputType<UpdateReturnableDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateReturnableDto> descriptor)
        {
            descriptor.Name("UpdateReturnableInput");
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.WholeSalePrice);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
