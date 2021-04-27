using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class RewardSortType : SortInputType<RewardDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.Name("RewardSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
        }
    }
}
