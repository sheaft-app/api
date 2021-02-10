using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class DeliveryHourSortType : SortInputType<DeliveryHourDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Day);
            descriptor.Sortable(c => c.ExpectedDeliveryDate);
        }
    }
}
