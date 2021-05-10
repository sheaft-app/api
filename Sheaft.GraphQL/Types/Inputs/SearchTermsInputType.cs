using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchTermsInputType : SheaftInputType<SearchTermsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchTermsDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SearchTermsInput");
            
            descriptor
                .Field(c => c.Latitude)
                .Name("latitude");
                
            descriptor
                .Field(c => c.Longitude)
                .Name("longitude");
                
            descriptor
                .Field(c => c.MaxDistance)
                .Name("maxDistance");
                
            descriptor
                .Field(c => c.Page)
                .Name("page");
                
            descriptor
                .Field(c => c.Sort)
                .Name("sort");
                
            descriptor
                .Field(c => c.Tags)
                .Name("tags");
                
            descriptor
                .Field(c => c.Take)
                .Name("take");
                
            descriptor
                .Field(c => c.Text)
                .Name("text");
        }
    }
}
