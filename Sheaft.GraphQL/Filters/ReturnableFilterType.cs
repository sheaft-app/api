using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class ReturnableFilterType : FilterInputType<ReturnableDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
