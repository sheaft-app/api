using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class QuickOrderProductQuantityFilterType : FilterInputType<QuickOrderProductQuantityDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<QuickOrderProductQuantityDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
        }
    }
}
