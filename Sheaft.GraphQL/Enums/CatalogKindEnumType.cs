using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class CatalogKindEnumType : EnumType<CatalogKind>
    {
        protected override void Configure(IEnumTypeDescriptor<CatalogKind> descriptor)
        {
            descriptor.Value(CatalogKind.Consumers).Name("CONSUMERS");
            descriptor.Value(CatalogKind.Stores).Name("STORES");
        }
    }
}