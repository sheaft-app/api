using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdateBusinessLegalsInputType : SheaftInputType<UpdateBusinessLegalInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBusinessLegalInput> descriptor)
        {
            descriptor.Field(c => c.Email);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<LegalKindEnumType>>();

            descriptor.Field(c => c.Owner)
                .Type<NonNullType<OwnerInputType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();
        }
    }
}
