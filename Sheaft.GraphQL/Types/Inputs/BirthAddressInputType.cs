using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BirthAddressInputType : SheaftInputType<BirthAddressDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BirthAddressDto> descriptor)
        {
            descriptor.Name("BirthAddressInput");
            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }
}
