using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class ProducerSummaryFilterType : FilterInputType<ProducerSummaryDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ProducerSummaryDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
