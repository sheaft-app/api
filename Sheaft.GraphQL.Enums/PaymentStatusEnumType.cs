using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class PaymentStatusEnumType : EnumType<PaymentStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<PaymentStatus> descriptor)
        {
            descriptor.Value(PaymentStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(PaymentStatus.Expired).Name("EXPIRED");
            descriptor.Value(PaymentStatus.Validated).Name("VALIDATED");
            descriptor.Value(PaymentStatus.Waiting).Name("WAITING");
        }
    }  
}
