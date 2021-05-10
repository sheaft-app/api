using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchProductsInputType : SheaftInputType<SearchProductsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchProductsDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SearchProductsInput");
            
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
                .Field(c => c.ProducerId)
                .Name("producerId")
                .ID(nameof(Producer));
                
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
