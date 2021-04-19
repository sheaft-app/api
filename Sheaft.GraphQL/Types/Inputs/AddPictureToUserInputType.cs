using HotChocolate.Types;
using Sheaft.Mediatr.ProfileInformation.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToUserInputType : SheaftInputType<AddPictureToUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToUserCommand> descriptor)
        {
            descriptor.Name("AddPictureToUserInput");
            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Picture);
        }
    }
}