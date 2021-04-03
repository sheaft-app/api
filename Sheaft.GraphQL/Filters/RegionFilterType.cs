using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class RegionFilterType : FilterInputType<RegionDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.Name("RegionFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Code).AllowEquals();
        }
    }
}
