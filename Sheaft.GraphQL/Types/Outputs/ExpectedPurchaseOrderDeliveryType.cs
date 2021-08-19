using HotChocolate.Types;
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
                .Field(c => c.Name)
                .Name("name");
                
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");

            descriptor
                .Field(c => c.Address)
                .Name("address");
            
            descriptor
                .Field(c => c.DeliveryFeesWholeSalePrice)
                .Name("deliveryFeesWholeSalePrice");
            
            descriptor
                .Field(c => c.DeliveryFeesVatPrice)
                .Name("deliveryFeesVatPrice");
            
            descriptor
                .Field(c => c.DeliveryFeesOnSalePrice)
                .Name("deliveryFeesOnSalePrice");
        }
    }
}