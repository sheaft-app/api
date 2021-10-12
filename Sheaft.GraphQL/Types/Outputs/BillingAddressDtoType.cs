using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BillingAddressDtoType : SheaftOutputType<BillingAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BillingAddressDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("BillingAddress");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Line1)
                .Name("line1");
            
            descriptor
                .Field(c => c.Line2)
                .Name("line2");
            
            descriptor
                .Field(c => c.Zipcode)
                .Name("zipcode");
            
            descriptor
                .Field(c => c.City)
                .Name("city");
            
            descriptor
                .Field(c => c.Country)
                .Name("country");
        }
    }
}