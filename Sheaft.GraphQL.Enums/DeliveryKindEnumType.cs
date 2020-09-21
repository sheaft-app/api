using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class DeliveryKindEnumType : EnumType<DeliveryKind>
    {
        protected override void Configure(IEnumTypeDescriptor<DeliveryKind> descriptor)
        {
            descriptor.Value(DeliveryKind.Collective).Name("COLLECTIVE");
            descriptor.Value(DeliveryKind.ExternalToConsumer).Name("EXTERNAL_TO_CONSUMER");
            descriptor.Value(DeliveryKind.ExternalToStore).Name("EXTERNAL_TO_STORE");
            descriptor.Value(DeliveryKind.Farm).Name("FARM");
            descriptor.Value(DeliveryKind.Market).Name("MARKET");
            descriptor.Value(DeliveryKind.ProducerToConsumer).Name("PRODUCER_TO_CONSUMER");
            descriptor.Value(DeliveryKind.ProducerToStore).Name("PRODUCER_TO_STORE");
            descriptor.Value(DeliveryKind.Withdrawal).Name("WITHDRAWAL");
        }
    }
}
