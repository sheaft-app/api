using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class ProductFilterType : FilterInputType<ProductDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("ProductFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Reference);
        }
    }    
}
