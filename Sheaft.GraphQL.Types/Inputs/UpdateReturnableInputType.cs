using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdateReturnableInputType : SheaftInputType<UpdateReturnableInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateReturnableInput> descriptor)
        {
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
