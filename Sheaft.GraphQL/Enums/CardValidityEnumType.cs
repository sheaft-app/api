using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class CardValidityEnumType : EnumType<CardValidity>
    {
        protected override void Configure(IEnumTypeDescriptor<CardValidity> descriptor)
        {
            descriptor.Value(CardValidity.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(CardValidity.Unknown).Name("UNKNOWN");
            descriptor.Value(CardValidity.Valid).Name("VALID");
            descriptor.Value(CardValidity.Invalid).Name("INVALID");
        }
    }
}