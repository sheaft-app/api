using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class QuickOrderFilterType : FilterInputType<QuickOrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.Name("QuickOrderFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.IsDefault).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
