using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ChangeAgreementDeliveryInputType : SheaftInputType<ChangeAgreementDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ChangeAgreementDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ChangeAgreementDeliveryInput");
            
            descriptor
                .Field(c => c.DeliveryId)
                .Name("deliveryId")
                .ID(nameof(DeliveryMode));
            
            descriptor
                .Field(c => c.AgreementId)
                .Name("agreementId")
                .ID(nameof(Agreement));
        }
    }
}