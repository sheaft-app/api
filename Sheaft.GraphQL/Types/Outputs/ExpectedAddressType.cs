using HotChocolate.Types;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ExpectedAddressType : SheaftOutputType<ExpectedAddress>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedAddress> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Line1)
                .Name("line1");
            
            descriptor
                .Field(c => c.Line2)
                .Name("line2");
            
            descriptor
                .Field(c => c.Longitude)
                .Name("longitude");
            
            descriptor
                .Field(c => c.Latitude)
                .Name("latitude");
            
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