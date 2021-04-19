using HotChocolate.Types;
using Sheaft.Mediatr.ProfileInformation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveUserPicturesInputType : SheaftInputType<RemoveUserPicturesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveUserPicturesCommand> descriptor)
        {
            descriptor.Name("RemoveUserPicturesInput");
            descriptor.Field(c => c.PictureIds)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.UserId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}