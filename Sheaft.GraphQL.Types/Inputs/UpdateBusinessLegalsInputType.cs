using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdateBusinessLegalsInputType : SheaftInputType<UpdateBusinessLegalInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBusinessLegalInput> descriptor)
        {
            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<LegalKindEnumType>>();

            descriptor.Field(c => c.Owner)
                .Type<NonNullType<OwnerInputType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.VatIdentifier)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();
        }
    }
}
