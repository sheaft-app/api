using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AddressType : ObjectType<AddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.Field(c => c.Line1);
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Zipcode);
            descriptor.Field(c => c.City);
            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }
}
