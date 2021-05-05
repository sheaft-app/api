using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddressInputType : SheaftInputType<AddressDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddressDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("AddressInput");

            descriptor
                .Field(c => c.Line1)
                .Name("line1")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.Line2)
                .Name("line2");

            descriptor
                .Field(c => c.Zipcode)
                .Name("zipcode")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.City)
                .Name("city")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Country)
                .Name("country");
            
            descriptor
                .Field(c => c.Longitude)
                .Name("longitude");
            
            descriptor
                .Field(c => c.Latitude)
                .Name("latitude");
        }
    }
}
