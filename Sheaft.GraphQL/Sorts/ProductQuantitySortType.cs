using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class ProductQuantitySortType : SortInputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.Name("ProductQuantitySort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
        }
    }    
}
