using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class WithholdingFilterType : FilterInputType<WithholdingDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<WithholdingDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
            descriptor.Filter(c => c.CreatedOn).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.CreatedOn).AllowLowerThanOrEquals();
            descriptor.Filter(c => c.UpdatedOn).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.UpdatedOn).AllowLowerThanOrEquals();
        }
    }
}