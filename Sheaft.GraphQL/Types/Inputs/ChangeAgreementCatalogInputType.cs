using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ChangeAgreementCatalogInputType : SheaftInputType<ChangeAgreementCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ChangeAgreementCatalogCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ChangeAgreementCatalogInput");
            
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