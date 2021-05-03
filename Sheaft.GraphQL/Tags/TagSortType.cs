using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Tags
{
    public class TagSortType : SortInputType<TagDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("TagSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Kind);
        }
    }
}
