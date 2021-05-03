using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Ratings
{
    public class RatingSortType : SortInputType<RatingDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.Name("RatingSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
