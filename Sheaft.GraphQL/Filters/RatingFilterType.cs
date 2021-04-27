using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class RatingFilterType : FilterInputType<RatingDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.Name("RatingFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
