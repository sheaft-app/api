using HotChocolate.Types;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteUserInputType : SheaftInputType<DeleteUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteUserCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteUserInput");

            descriptor
                .Field(c => c.Reason)
                .Name("reason");
        }
    }
}