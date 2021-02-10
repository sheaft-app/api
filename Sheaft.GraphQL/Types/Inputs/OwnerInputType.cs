using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class OwnerInputType : SheaftInputType<OwnerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<OwnerInput> descriptor)
        {
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.BirthDate);
            descriptor.Field(c => c.Nationality);
            descriptor.Field(c => c.CountryOfResidence);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();
        }
    }
}
