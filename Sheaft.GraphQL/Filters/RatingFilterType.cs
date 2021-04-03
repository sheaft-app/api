using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class RatingFilterType : FilterInputType<RatingDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.Name("RatingFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Value).AllowGreaterThan();
            descriptor.Filter(c => c.Value).AllowNotGreaterThanOrEquals();
            descriptor.Filter(c => c.Value).AllowLowerThan();
            descriptor.Filter(c => c.Value).AllowLowerThanOrEquals();
            descriptor.Filter(c => c.CreatedOn).AllowGreaterThan();
            descriptor.Filter(c => c.CreatedOn).AllowNotGreaterThanOrEquals();
            descriptor.Filter(c => c.CreatedOn).AllowLowerThan();
            descriptor.Filter(c => c.CreatedOn).AllowLowerThanOrEquals();
        }
    }
}
