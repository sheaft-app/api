using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToUserProfileInputType : SheaftInputType<AddPictureToUserProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToUserProfileInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            
            descriptor.Field(c => c.Picture)
                .Type<NonNullType<PictureInputType>>();
        }
    }
}