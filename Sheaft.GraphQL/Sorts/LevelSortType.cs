using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class LevelSortType : SortInputType<LevelDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<LevelDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Number);
        }
    }
}
