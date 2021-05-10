using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AssignCatalogToAgreementInputType : SheaftInputType<AssignCatalogToAgreementCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AssignCatalogToAgreementCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("AssignCatalogToAgreementInput");
            
            descriptor
                .Field(c => c.CatalogId)
                .Name("catalogId")
                .ID(nameof(Catalog));
            
            descriptor
                .Field(c => c.AgreementId)
                .Name("agreementId")
                .ID(nameof(Agreement));
        }
    }
}