using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class TagKindEnumType : EnumType<TagKind>
    {
        protected override void Configure(IEnumTypeDescriptor<TagKind> descriptor)
        {
            descriptor.Value(TagKind.Allergen).Name("ALLERGEN");
            descriptor.Value(TagKind.Category).Name("CATEGORY");
            descriptor.Value(TagKind.Diet).Name("DIET");
            descriptor.Value(TagKind.Ingredient).Name("INGREDIENT");
            descriptor.Value(TagKind.Label).Name("LABEL");
        }
    }
}
