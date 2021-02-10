using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class DocumentKindEnumType : EnumType<DocumentKind>
    {
        protected override void Configure(IEnumTypeDescriptor<DocumentKind> descriptor)
        {
            descriptor.Value(DocumentKind.AddressProof).Name("ADDRESS_PROOF");
            descriptor.Value(DocumentKind.AssociationProof).Name("ASSOCIATION_PROOF");
            descriptor.Value(DocumentKind.IdentityProof).Name("IDENTITY_PROOF");
            descriptor.Value(DocumentKind.RegistrationProof).Name("REGISTRATION_PROOF");
            descriptor.Value(DocumentKind.ShareholderProof).Name("SHAREHOLDER_PROOF");
        }
    }
}
