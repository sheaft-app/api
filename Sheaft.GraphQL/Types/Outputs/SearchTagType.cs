using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SearchTagType : SheaftOutputType<TagDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("SearchTagType");

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
