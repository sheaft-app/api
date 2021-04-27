using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class LevelSortType : SortInputType<LevelDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<LevelDto> descriptor)
        {
            descriptor.Name("LevelSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Number);
        }
    }
}
