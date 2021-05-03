using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Producers
{
    public class SearchProducerSortType : SortInputType<ProducerDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProducerDto> descriptor)
        {
            descriptor.Name("SearchProducerSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
        }
    }
}
