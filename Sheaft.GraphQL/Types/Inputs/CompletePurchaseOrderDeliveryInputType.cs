using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrderDelivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CompletePurchaseOrderDeliveryInputType : SheaftInputType<CompletePurchaseOrderDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CompletePurchaseOrderDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CompletePurchaseOrderDeliveryInput");

            descriptor
                .Field(c => c.PurchaseOrderDeliveryId)
                .ID(nameof(PurchaseOrderDelivery))
                .Name("id");

            descriptor
                .Field(c => c.ReceptionedBy)
                .Name("receptionedBy");

            descriptor
                .Field(c => c.Comment)
                .Name("comment");
        }
    }
}