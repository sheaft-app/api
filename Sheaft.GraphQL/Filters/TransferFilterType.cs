using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class TransferFilterType : FilterInputType<TransferDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TransferDto> descriptor)
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