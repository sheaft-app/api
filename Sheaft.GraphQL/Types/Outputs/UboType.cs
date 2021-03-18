using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class UboType : SheaftOutputType<UboDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UboDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.BirthDate);

            descriptor.Field(c => c.Nationality)
                .Type<NonNullType<CountryIsoCodeEnumType>>();

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.BirthPlace)
                .Type<BirthAddressType>();
        }
    }
}
