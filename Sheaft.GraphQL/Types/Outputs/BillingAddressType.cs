using HotChocolate.Types;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BillingAddressType : SheaftOutputType<BillingAddress>
    {
        protected override void Configure(IObjectTypeDescriptor<BillingAddress> descriptor)
        {
            base.Configure(descriptor);
            
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