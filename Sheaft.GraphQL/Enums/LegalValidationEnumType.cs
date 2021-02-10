using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class LegalValidationEnumType : EnumType<LegalValidation>
    {
        protected override void Configure(IEnumTypeDescriptor<LegalValidation> descriptor)
        {
            descriptor.Value(LegalValidation.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(LegalValidation.Light).Name("LIGHT");
            descriptor.Value(LegalValidation.Regular).Name("REGULAR");
        }
    }
}
