using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AssignCatalogToAgreementInputType : SheaftInputType<AssignCatalogToAgreementCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AssignCatalogToAgreementCommand> descriptor)
        {
            descriptor.Name("AssignCatalogToAgreementInput");
            descriptor.Field(c => c.CatalogId)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.AgreementId)
                .Type<NonNullType<IdType>>();
        }
    }
}