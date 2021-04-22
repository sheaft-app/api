using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AgreementCatalogType : ObjectType<AgreementCatalogDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AgreementCatalogDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name);
        }
    }
}