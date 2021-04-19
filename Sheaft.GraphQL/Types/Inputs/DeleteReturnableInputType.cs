using HotChocolate.Types;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteReturnableInputType : SheaftInputType<DeleteReturnableCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteReturnableCommand> descriptor)
        {
            descriptor.Name("DeleteReturnableInput");
            descriptor.Field(c => c.ReturnableId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}