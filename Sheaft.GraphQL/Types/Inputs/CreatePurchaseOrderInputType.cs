using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreatePurchaseOrderInputType : SheaftInputType<CreatePurchaseOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePurchaseOrderCommand> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("CreatePurchaseOrderInput");
            
            descriptor
                .Field(c => c.ClientId)
                .Name("storeId")
                .ID(nameof(Store));
            
            descriptor
                .Field(c => c.DeliveryModeId)
                .Name("deliveryModeId")
                .ID(nameof(DeliveryMode));
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.To)
                .Name("to");
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
            
            descriptor
                .Field(c => c.SkipNotification)
                .Name("skipNotification");
            
            descriptor
                .Field(c => c.Products)
                .Type<ListType<ResourceIdQuantityInputType>>()
                .Name("products");
        }
    }
}