using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types
{
    public class TagType : SheaftOutputType<TagDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Kind).Type<NonNullType<TagKindEnumType>>();
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
