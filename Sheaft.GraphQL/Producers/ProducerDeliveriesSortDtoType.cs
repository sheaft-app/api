using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Producers
{
    public class ProducerDeliveriesSortDtoType : SortInputType<ProducerDeliveriesDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProducerDeliveriesDto> descriptor)
        {
            descriptor.Name("ProducerSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
        }
    }
}
