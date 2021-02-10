using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
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
