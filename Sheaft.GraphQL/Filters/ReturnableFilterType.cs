using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class ReturnableFilterType : FilterInputType<ReturnableDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ReturnableDto> descriptor)
        {
            descriptor.Name("ReturnableFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
        }
    }
}
