using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Payouts
{
    public class PayoutFilterType : FilterInputType<PayoutDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<PayoutDto> descriptor)
        {
            descriptor.Name("PayoutFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
        }
    }
}