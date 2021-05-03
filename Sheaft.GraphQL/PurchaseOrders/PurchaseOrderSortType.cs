using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.PurchaseOrders
{
    public class PurchaseOrderSortType : SortInputType<PurchaseOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.Name("PurchaseOrderSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalOnSalePrice);
        }
    }
}
