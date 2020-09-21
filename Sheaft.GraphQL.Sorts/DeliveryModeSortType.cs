using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class DeliveryModeSortType : SortInputType<DeliveryModeDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.Name);
        }
    }
}
