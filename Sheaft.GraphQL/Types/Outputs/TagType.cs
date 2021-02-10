using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
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
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Icon);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
