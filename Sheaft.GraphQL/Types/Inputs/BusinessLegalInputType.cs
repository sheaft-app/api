using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BusinessLegalInputType : SheaftInputType<BusinessLegalInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<BusinessLegalInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("BusinessLegalInput");

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
                .Field(c => c.LegalKind)
                .Name("kind");
            
            descriptor
                .Field(c => c.RegistrationKind)
                .Name("registrationKind");
            
            descriptor
                .Field(c => c.RegistrationCity)
                .Name("registrationCity");
            
            descriptor
                .Field(c => c.RegistrationCode)
                .Name("registrationCode");

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