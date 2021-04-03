using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class RewardFilterType : FilterInputType<RewardDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.Name("RewardFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
