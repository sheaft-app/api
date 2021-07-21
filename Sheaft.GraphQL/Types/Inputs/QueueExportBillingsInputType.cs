using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Billing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportBillingsInputType : SheaftInputType<QueueExportBillingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportBillingsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("QueueExportBillingsInput");

            descriptor
                .Field(c => c.DeliveryIds)
                .ID(nameof(Delivery))
                .Name("ids");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
        }
    }
}