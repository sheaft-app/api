using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SearchProductAddressType : ObjectType<AddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressDto> descriptor)
        {
            descriptor.Name("SearchProductAddressType");

            descriptor.Field(c => c.Longitude).Type<NonNullType<DecimalType>>();
            descriptor.Field(c => c.Latitude).Type<NonNullType<DecimalType>>();

            descriptor.Field(c => c.Zipcode)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.City)
                .Type<NonNullType<StringType>>();
        }
    }
}
