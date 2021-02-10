using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class ProducerSummarySortType : SortInputType<ProducerSummaryDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProducerSummaryDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
}
