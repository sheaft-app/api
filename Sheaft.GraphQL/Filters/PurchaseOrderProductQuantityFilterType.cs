using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class PurchaseOrderProductQuantityFilterType : FilterInputType<PurchaseOrderProductQuantityDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PurchaseOrderProductQuantityDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
        }
    }
}
