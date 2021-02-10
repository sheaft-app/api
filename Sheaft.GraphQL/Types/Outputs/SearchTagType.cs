using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SearchTagType : SheaftOutputType<TagDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("SearchTagDto");

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
