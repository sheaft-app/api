using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.ProfileInformation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveUserPicturesInputType : SheaftInputType<RemoveUserPicturesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveUserPicturesCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RemoveUserPicturesInput");
            
            descriptor.Field(c => c.PictureIds)
                .Name("pictureIds")
                .ID(nameof(ProfilePicture));

            descriptor.Field(c => c.UserId)
                .Name("id")
                .ID(nameof(User));
        }
    }
}