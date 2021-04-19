using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateReturnableInputType : SheaftInputType<CreateReturnableCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateReturnableCommand> descriptor)
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
