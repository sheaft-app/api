using HotChocolate.Types;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBusinessLegalInputType : SheaftInputType<CreateBusinessLegalCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBusinessLegalCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateBusinessLegalInput");
            
            descriptor
                .Field(c => c.VatIdentifier)
                .Name("vatIdentifier");

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Siret)
                .Name("siret")
                .Type<NonNullType<StringType>>();

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
        }
    }
}
