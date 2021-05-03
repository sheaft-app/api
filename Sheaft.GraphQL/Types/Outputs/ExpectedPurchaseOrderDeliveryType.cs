using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ExpectedPurchaseOrderDeliveryType : SheaftOutputType<ExpectedPurchaseOrderDelivery>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedPurchaseOrderDelivery> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
                
            descriptor
                .Field(c => c.DeliveryStartedOn)
                .Name("deliveryStartedOn");
                
            descriptor
                .Field(c => c.DeliveredOn)
                .Name("deliveredOn");
                
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Name)
                .Name("name");

            descriptor
                .Field(c => c.Address)
                .Name("address");
        }
    }
}
