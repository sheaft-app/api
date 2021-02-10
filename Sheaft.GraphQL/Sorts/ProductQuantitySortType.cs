using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class ProductQuantitySortType : SortInputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }    
}
