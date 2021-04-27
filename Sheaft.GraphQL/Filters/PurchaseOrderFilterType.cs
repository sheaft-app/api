using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class PurchaseOrderFilterType : FilterInputType<PurchaseOrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.Name("PurchaseOrderFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.Status);
        }
    }
}
