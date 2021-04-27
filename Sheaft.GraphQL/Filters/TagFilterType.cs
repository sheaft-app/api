using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class TagFilterType : FilterInputType<TagDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("TagFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
        }
    }
}
