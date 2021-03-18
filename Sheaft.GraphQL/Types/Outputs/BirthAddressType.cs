using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BirthAddressType : ObjectType<BirthAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BirthAddressDto> descriptor)
        {
            descriptor.Field(c => c.City);
            descriptor.Field(c => c.Country)
                .Type<NonNullType<CountryIsoCodeEnumType>>();
        }
    }
}
