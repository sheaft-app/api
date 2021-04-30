using HotChocolate.Data.Filters;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Filters
{
    public class TagFilterType : FilterInputType<Tag>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Tag> descriptor)
        {
            descriptor.Name("TagFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
        }
    }
}
