using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.ProfileInformation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToUserInputType : SheaftInputType<AddPictureToUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToUserCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("AddPictureToUserInput");
            
            descriptor
                .Field(c => c.UserId)
                .Name("id")
                .ID(nameof(User));

            descriptor
                .Field(c => c.Picture)
                .Name("picture");
        }
    }
}