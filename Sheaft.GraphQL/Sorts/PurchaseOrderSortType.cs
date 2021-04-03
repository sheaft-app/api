using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class PurchaseOrderSortType : SortInputType<PurchaseOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.Name("PurchaseOrderSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Reference);
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.TotalWholeSalePrice);
            descriptor.Sortable(c => c.TotalOnSalePrice);
        }
    }
}
