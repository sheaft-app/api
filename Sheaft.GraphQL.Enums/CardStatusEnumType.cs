using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class CardStatusEnumType : EnumType<CardStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<CardStatus> descriptor)
        {
            descriptor.Value(CardStatus.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(CardStatus.Created).Name("CREATED");
            descriptor.Value(CardStatus.Validated).Name("VALIDATED");
            descriptor.Value(CardStatus.Error).Name("ERROR");
        }
    } 
}
