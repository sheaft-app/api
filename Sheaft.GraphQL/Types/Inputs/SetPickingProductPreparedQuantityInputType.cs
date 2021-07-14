using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetPickingProductPreparedQuantityInputType : SheaftInputType<SetPickingProductPreparedQuantityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetPickingProductPreparedQuantityCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetPickingProductPreparedQuantityInput");

            descriptor
                .Field(c => c.PickingId)
                .ID(nameof(Picking))
                .Name("id");
            
            descriptor
                .Field(c => c.ProductId)
                .ID(nameof(Product))
                .Name("productId");
            
            descriptor
                .Field(c => c.Completed)
                .Name("completed");
            
            descriptor
                .Field(c => c.PreparedBy)
                .Name("preparedBy");
            
            descriptor
                .Field(c => c.Quantities)
                .Type<ListType<PickingPurchaseOrderProductQuantityInputType>>()
                .Name("preparedQuantities");
            
            descriptor
                .Field(c => c.Batches)
                .ID(nameof(Batch))
                .Name("batches");
        }
    }
}