using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class ConditioningKindEnumType : EnumType<ConditioningKind>
    {
        protected override void Configure(IEnumTypeDescriptor<ConditioningKind> descriptor)
        {
            descriptor.Value(ConditioningKind.Bouquet).Name("BOUQUET");
            descriptor.Value(ConditioningKind.Bulk).Name("BULK");
            descriptor.Value(ConditioningKind.Box).Name("BOX");
            descriptor.Value(ConditioningKind.Bunch).Name("BUNCH");
            descriptor.Value(ConditioningKind.Piece).Name("PIECE");
            descriptor.Value(ConditioningKind.Basket).Name("BASKET");
            descriptor.Value(ConditioningKind.Not_Specified).Name("NOT_SPECIFIED");
        }
    }
}
