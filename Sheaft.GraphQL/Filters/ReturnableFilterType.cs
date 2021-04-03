using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class ReturnableFilterType : FilterInputType<ReturnableDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.Name("ReturnableFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
