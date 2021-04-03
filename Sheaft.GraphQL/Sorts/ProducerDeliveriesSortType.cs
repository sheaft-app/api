using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class ProducerDeliveriesSortType : SortInputType<ProducerDeliveriesDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProducerDeliveriesDto> descriptor)
        {
            descriptor.Name("ProducerSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
}
