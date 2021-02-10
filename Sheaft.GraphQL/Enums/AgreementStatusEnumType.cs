using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class AgreementStatusEnumType : EnumType<AgreementStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<AgreementStatus> descriptor)
        {
            descriptor.Value(AgreementStatus.Accepted).Name("ACCEPTED");
            descriptor.Value(AgreementStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(AgreementStatus.Refused).Name("REFUSED");
            descriptor.Value(AgreementStatus.WaitingForProducerApproval).Name("WAITING_FOR_PRODUCER_APPROVAL");
            descriptor.Value(AgreementStatus.WaitingForStoreApproval).Name("WAITING_FOR_STORE_APPROVAL");
        }
    }
}
