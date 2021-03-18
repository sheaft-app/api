using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBusinessLegalsInputType : SheaftInputType<CreateBusinessLegalDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBusinessLegalDto> descriptor)
        {
            descriptor.Name("CreateBusinessLegalInput");
            descriptor.Field(c => c.VatIdentifier);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<LegalKindEnumType>>();

            descriptor.Field(c => c.Owner)
                .Type<NonNullType<CreateOwnerInputType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();
        }
    }
}
