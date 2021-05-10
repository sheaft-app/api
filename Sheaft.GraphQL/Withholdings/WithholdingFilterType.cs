using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Withholdings
{
    public class WithholdingFilterType : FilterInputType<WithholdingDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<WithholdingDto> descriptor)
        {
            descriptor.Name("WithholdingFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
        }
    }
}