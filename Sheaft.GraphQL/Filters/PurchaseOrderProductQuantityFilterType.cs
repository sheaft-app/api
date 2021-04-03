using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class PurchaseOrderProductQuantityFilterType : FilterInputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.Name("PurchaseOrderProductQuantityFilter");
            descriptor.BindFieldsExplicitly();
        }
    }
}
