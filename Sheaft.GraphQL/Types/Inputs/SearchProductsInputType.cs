using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchProductsInputType : SheaftInputType<SearchProductsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchProductsDto> descriptor)
        {
            descriptor.Name("SearchProductsInput");
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.MaxDistance);
            descriptor.Field(c => c.ProducerId);
            descriptor.Field(c => c.Page);
            descriptor.Field(c => c.Sort);
            descriptor.Field(c => c.Tags);
            descriptor.Field(c => c.Take);
            descriptor.Field(c => c.Text);
        }
    }
}
