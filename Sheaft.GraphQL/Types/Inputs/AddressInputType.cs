using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddressInputType : SheaftInputType<AddressDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.Name("AddressInput");
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Longitude);
            descriptor.Field(c => c.Latitude);

            descriptor.Field(c => c.Line1)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Zipcode)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Country);
        }
    }
}
