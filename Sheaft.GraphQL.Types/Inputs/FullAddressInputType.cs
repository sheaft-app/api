using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class FullAddressInputType : SheaftInputType<FullAddressInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FullAddressInput> descriptor)
        {
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Latitude);
            descriptor.Field(c => c.Longitude);

            descriptor.Field(c => c.Line1)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Zipcode)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }
}
