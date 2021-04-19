using HotChocolate.Types;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveUserInputType : SheaftInputType<RemoveUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveUserCommand> descriptor)
        {
            descriptor.Name("RemoveUserInput");
            descriptor.Field(c => c.Reason);

            descriptor.Field(c => c.UserId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}