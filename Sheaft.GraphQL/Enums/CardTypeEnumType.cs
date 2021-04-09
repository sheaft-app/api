using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class CardTypeEnumType : EnumType<CardType>
    {
        protected override void Configure(IEnumTypeDescriptor<CardType> descriptor)
        {
            descriptor.Value(CardType.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(CardType.AMEX).Name("AMEX");
            descriptor.Value(CardType.BCMC).Name("BCMC");
            descriptor.Value(CardType.CB_VISA_MASTERCARD).Name("CB_VISA_MASTERCARD");
            descriptor.Value(CardType.DINERS).Name("DINERS");
            descriptor.Value(CardType.IDEAL).Name("IDEAL");
            descriptor.Value(CardType.MAESTRO).Name("MAESTRO");
            descriptor.Value(CardType.MASTERPASS).Name("MASTERPASS");
            descriptor.Value(CardType.P24).Name("P24");
            descriptor.Value(CardType.PAYLIB).Name("PAYLIB");
        }
    }
}