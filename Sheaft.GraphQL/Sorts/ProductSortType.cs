using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class ProductSortType : SortInputType<ProductDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.CreatedOn);
            descriptor.Sortable(c => c.WholeSalePrice);
            descriptor.Sortable(c => c.OnSalePrice);
        }
    }
}
