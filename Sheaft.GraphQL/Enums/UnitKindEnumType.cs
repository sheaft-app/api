using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class UnitKindEnumType : EnumType<UnitKind>
    {
        protected override void Configure(IEnumTypeDescriptor<UnitKind> descriptor)
        {
            descriptor.Value(UnitKind.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(UnitKind.g).Name("G");
            descriptor.Value(UnitKind.kg).Name("KG");
            descriptor.Value(UnitKind.l).Name("L");
            descriptor.Value(UnitKind.ml).Name("ML");
        }
    }
}
