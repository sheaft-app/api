using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class PurchaseOrderSortType : SortInputType<PurchaseOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Reference);
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.TotalWholeSalePrice);
            descriptor.Sortable(c => c.TotalOnSalePrice);
        }
    }
}
