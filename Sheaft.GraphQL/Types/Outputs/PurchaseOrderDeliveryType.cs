using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.PurchaseOrders;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PurchaseOrderDeliveryType : SheaftOutputType<PurchaseOrderDelivery>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrderDelivery> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<PurchaseOrderDeliveriesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
                
            descriptor
                .Field(c => c.DeliveredOn)
                .Name("deliveredOn");
                
            descriptor
                .Field(c => c.From)
                .Name("from");

            descriptor
                .Field(c => c.Day)
                .Name("day");
                
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

            descriptor
                .Field(c => c.Position)
                .Name("position");
        }
    }
}
