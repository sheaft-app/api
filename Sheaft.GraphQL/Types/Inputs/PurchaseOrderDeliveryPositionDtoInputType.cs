using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PurchaseOrderDeliveryPositionDtoInputType : SheaftInputType<PurchaseOrderDeliveryPositionDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PurchaseOrderDeliveryPositionDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PurchaseOrderDeliveryPositionInput");
            
            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(PurchaseOrderDelivery))
                .Name("id");
            
            descriptor
                .Field(c => c.Position)
                .Name("position");
        }
    }
}