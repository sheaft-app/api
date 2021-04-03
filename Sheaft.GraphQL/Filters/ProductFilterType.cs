using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class ProductFilterType : FilterInputType<ProductDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("ProductFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Available).AllowEquals();
            descriptor.Filter(c => c.Name).AllowContains();
            descriptor.Filter(c => c.Reference).AllowContains();
        }
    }    
}
