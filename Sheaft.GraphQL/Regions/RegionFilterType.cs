using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Regions
{
    public class RegionFilterType : FilterInputType<RegionDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.Name("RegionFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Code);
        }
    }
}
