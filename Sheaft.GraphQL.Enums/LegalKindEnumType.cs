using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class LegalKindEnumType : EnumType<LegalKind>
    {
        protected override void Configure(IEnumTypeDescriptor<LegalKind> descriptor)
        {
            descriptor.Value(LegalKind.Natural).Name("NATURAL");
            descriptor.Value(LegalKind.Business).Name("BUSINESS");
            descriptor.Value(LegalKind.Individual).Name("INDIVIDUAL");
            descriptor.Value(LegalKind.Organization).Name("ORGANIZATION");
        }
    }
}
