using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class QuickOrderProductQuantityFilterType : FilterInputType<QuickOrderProductQuantityDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<QuickOrderProductQuantityDto> descriptor)
        {
            descriptor.Name("QuickOrderProductQuantityFilter");
            descriptor.BindFieldsExplicitly();
        }
    }
}
