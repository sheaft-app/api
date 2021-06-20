using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AvailableClientDeliveryDtoType : SheaftOutputType<AvailableClientDeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AvailableClientDeliveryDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Id)
                .ID(nameof(User))
                .Name("id");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Position)
                .Name("position");

            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");
            
            descriptor
                .Field(c => c.PurchaseOrders)
                .Name("purchaseOrders")
                .Type<ListType<AvailablePurchaseOrderDtoType>>();
        }
    }
}