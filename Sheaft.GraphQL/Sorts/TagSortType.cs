using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class TagSortType : SortInputType<TagDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("TagSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
}
