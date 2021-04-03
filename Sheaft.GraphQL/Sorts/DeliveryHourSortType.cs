using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class DeliveryHourSortType : SortInputType<DeliveryHourDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.Name("DeliveryHourSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Day);
            descriptor.Sortable(c => c.ExpectedDeliveryDate);
        }
    }
}
