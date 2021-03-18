using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToInputType : SheaftInputType<AddPictureToResourceIdDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToResourceIdDto> descriptor)
        {
            descriptor.Name("AddPictureToResourceIdInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Picture);
        }
    }
}