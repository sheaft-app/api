using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class PurchaseOrderFilterType : FilterInputType<PurchaseOrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Id).AllowIn();
            descriptor.Filter(c => c.Reference).AllowContains();
            descriptor.Filter(c => c.Status).AllowIn();
        }
    }
}
