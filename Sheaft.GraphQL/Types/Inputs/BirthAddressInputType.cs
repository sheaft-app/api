using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BirthAddressInputType : SheaftInputType<BirthAddressDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BirthAddressDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("BirthAddressInput");
            
            descriptor.Field(c => c.City)
                .Name("city")
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Country)
                .Name("country");
        }
    }
}
