using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ClientDeliveryPositionDtoInputType : SheaftInputType<ClientDeliveryPositionDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ClientDeliveryPositionDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ClientDeliveryPositionDtoInput");
            
            descriptor
                .Field(c => c.ClientId)
                .ID(nameof(User))
                .Name("clientId");
            
            descriptor
                .Field(c => c.Position)
                .Name("position");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .ID(nameof(PurchaseOrder))
                .Name("purchaseOrderIds");
        }
    }
}