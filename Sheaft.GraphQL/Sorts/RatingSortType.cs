using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class RatingSortType : SortInputType<RatingDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.Name("RatingSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Value);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
