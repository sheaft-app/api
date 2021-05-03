using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Consumers
{
    public class ConsumerSortType : SortInputType<ConsumerDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ConsumerDto> descriptor)
        {
            descriptor.Name("ConsumerSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
