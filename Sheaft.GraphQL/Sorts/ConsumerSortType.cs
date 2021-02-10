using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class ConsumerSortType : SortInputType<ConsumerDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ConsumerDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
