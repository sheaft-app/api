using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ReturnableInputType : SheaftInputType<CreateReturnableInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateReturnableInput> descriptor)
        {
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.WholeSalePrice);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
