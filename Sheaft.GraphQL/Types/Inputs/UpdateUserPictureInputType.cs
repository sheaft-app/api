using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateUserPictureInputType : SheaftInputType<UpdateUserPreviewCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateUserPreviewCommand> descriptor)
        {
            descriptor.Name("UpdateUserPictureInput");
            descriptor.Field(c => c.Picture)
                .Type<PictureSourceInputType>();

            descriptor.Field(c => c.UserId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}
