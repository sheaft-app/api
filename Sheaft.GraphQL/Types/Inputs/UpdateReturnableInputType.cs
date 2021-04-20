using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateReturnableInputType : SheaftInputType<UpdateReturnableCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateReturnableCommand> descriptor)
        {
            descriptor.Name("UpdateReturnableInput");
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.WholeSalePrice);

            descriptor.Field(c => c.ReturnableId)
                .Name("id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
