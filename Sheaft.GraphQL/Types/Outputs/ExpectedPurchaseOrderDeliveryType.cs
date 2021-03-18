using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ExpectedPurchaseOrderDeliveryType : ObjectType<ExpectedPurchaseOrderDeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedPurchaseOrderDeliveryDto> descriptor)
        {
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.DeliveryStartedOn);
            descriptor.Field(c => c.DeliveredOn);
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();
        }
    }
}
