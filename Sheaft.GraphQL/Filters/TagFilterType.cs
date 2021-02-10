using HotChocolate.Types.Filters;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Filters
{
    public class TagFilterType : FilterInputType<TagDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TagDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Name).AllowContains();
        }
    }
}
