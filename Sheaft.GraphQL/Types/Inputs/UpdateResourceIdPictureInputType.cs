using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateResourceIdPictureInputType : SheaftInputType<UpdateResourceIdPictureDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateResourceIdPictureDto> descriptor)
        {
            descriptor.Name("UpdateResourceIdPictureInput");
            descriptor.Field(c => c.Picture)
                .Type<PictureSourceInputType>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
