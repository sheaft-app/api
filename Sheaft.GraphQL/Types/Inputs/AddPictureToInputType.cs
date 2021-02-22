using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToInputType : SheaftInputType<AddPictureToInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Picture);
        }
    }
}