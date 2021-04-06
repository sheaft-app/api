using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AssignCatalogToAgreementInputType : SheaftInputType<AssignCatalogToAgreementDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AssignCatalogToAgreementDto> descriptor)
        {
            descriptor.Name("AssignCatalogToAgreementInput");
            descriptor.Field(c => c.CatalogId)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.AgreementId)
                .Type<NonNullType<IdType>>();
        }
    }
}