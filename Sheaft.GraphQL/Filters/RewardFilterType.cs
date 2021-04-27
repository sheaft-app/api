using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class RewardFilterType : FilterInputType<RewardDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.Name("RewardFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
        }
    }
}
