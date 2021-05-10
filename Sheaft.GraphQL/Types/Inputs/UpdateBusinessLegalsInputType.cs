using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateBusinessLegalsInputType : SheaftInputType<UpdateBusinessLegalCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBusinessLegalCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateBusinessLegalInput");
            
            descriptor
                .Field(c => c.VatIdentifier)
                .Name("vatIdentifier")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.LegalId)
                .Name("id")
                .ID(nameof(BusinessLegal));

            descriptor
                .Field(c => c.Kind)
                .Name("kind");

            descriptor
                .Field(c => c.Owner)
                .Name("owner")
                .Type<NonNullType<CreateOwnerInputType>>();

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<AddressInputType>>();

            descriptor
                .Field(c => c.Siret)
                .Name("siret")
                .Type<NonNullType<StringType>>();
        }
    }
}
