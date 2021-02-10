using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdatePictureInputType : SheaftInputType<UpdatePictureInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePictureInput> descriptor)
        {
            descriptor.Field(c => c.OriginalPicture);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
