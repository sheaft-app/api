using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateUserPictureInputType : SheaftInputType<UpdateUserPreviewCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateUserPreviewCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateUserPictureInput");
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture");

            descriptor
                .Field(c => c.UserId)
                .Name("id")
                .ID(nameof(User));
        }
    }
}
