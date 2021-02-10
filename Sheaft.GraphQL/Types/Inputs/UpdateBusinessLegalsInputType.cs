using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateBusinessLegalsInputType : SheaftInputType<UpdateBusinessLegalInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBusinessLegalInput> descriptor)
        {
            descriptor.Field(c => c.VatIdentifier);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

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

            descriptor.Field(c => c.Siret)
                .Type<NonNullType<StringType>>();
        }
    }
}
