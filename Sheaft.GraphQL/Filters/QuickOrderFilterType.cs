using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class QuickOrderFilterType : FilterInputType<QuickOrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.Name("QuickOrderFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.IsDefault);
            descriptor.Field(c => c.Name);
        }
    }
}
