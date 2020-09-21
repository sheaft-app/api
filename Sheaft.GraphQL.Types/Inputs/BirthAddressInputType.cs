using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class BirthAddressInputType : SheaftInputType<BirthAddressInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BirthAddressInput> descriptor)
        {
            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }
}
