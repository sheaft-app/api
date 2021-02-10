using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
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
