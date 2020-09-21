using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class AddressInputType : SheaftInputType<AddressInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddressInput> descriptor)
        {
            descriptor.Field(c => c.Line2);

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
