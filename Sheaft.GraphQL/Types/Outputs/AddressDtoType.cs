using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AddressDtoType : SheaftOutputType<AddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("Address");
            
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