using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BillingAddressInputType : SheaftInputType<BillingAddressDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BillingAddressDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("BillingAddressInput");

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
            
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
        }
    }
}