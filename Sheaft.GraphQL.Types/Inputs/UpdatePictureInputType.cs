using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdatePictureInputType : SheaftInputType<UpdatePictureInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePictureInput> descriptor)
        {
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
