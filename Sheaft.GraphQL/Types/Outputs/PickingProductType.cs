using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Pickings;
using Sheaft.GraphQL.PurchaseOrders;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PickingProductType : SheaftOutputType<PickingProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<PickingProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PickingProduct");
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<PickingProductsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.ProductId)
                .ID(nameof(Product))
                .Name("productId");
            
            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");
            
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
                
            descriptor
                .Field(c => c.TotalWeight)
                .Name("totalWeight");
                
            descriptor
                .Field(c => c.UnitOnSalePrice)
                .Name("unitOnSalePrice");
                
            descriptor
                .Field(c => c.UnitVatPrice)
                .Name("unitVatPrice");
                
            descriptor
                .Field(c => c.UnitWholeSalePrice)
                .Name("unitWholeSalePrice");
                
            descriptor
                .Field(c => c.UnitVatPrice)
                .Name("unitVatPrice");
                
            descriptor
                .Field(c => c.UnitWeight)
                .Name("unitWeight");
                
            descriptor
                .Field(c => c.TotalWholeSalePrice)
                .Name("totalWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalVatPrice)
                .Name("totalVatPrice");
                
            descriptor
                .Field(c => c.TotalOnSalePrice)
                .Name("totalOnSalePrice");
                
            descriptor
                .Field(c => c.TotalReturnableWholeSalePrice)
                .Name("totalReturnableWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalReturnableVatPrice)
                .Name("totalReturnableVatPrice");
                
            descriptor
                .Field(c => c.TotalReturnableOnSalePrice)
                .Name("totalReturnableOnSalePrice");
                
            descriptor
                .Field(c => c.TotalProductWholeSalePrice)
                .Name("totalProductWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalProductVatPrice)
                .Name("totalProductVatPrice");
                
            descriptor
                .Field(c => c.TotalProductOnSalePrice)
                .Name("totalProductOnSalePrice");
            
            descriptor
                .Field(c => c.HasReturnable)
                .Name("isReturnable");
            
            descriptor
                .Field(c => c.QuantityPerUnit)
                .Name("quantityPerUnit");
                
            descriptor
                .Field(c => c.Conditioning)
                .Name("conditioning");
                
            descriptor
                .Field(c => c.Unit)
                .Name("unit");
            
            descriptor
                .Field(c => c.ReturnableId)
                .ID(nameof(Returnable))
                .Name("returnableId");
            
            descriptor
                .Field(c => c.PurchaseOrderId)
                .ID(nameof(PurchaseOrder))
                .Name("purchaseOrderId");
            
            descriptor
                .Field("purchaseOrder")
                .ResolveWith<PickingProductResolvers>(c => c.GetPurchaseOrder(default, default, default))
                .Type<PurchaseOrderType>();
            
            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Reference)
                .Name("reference")
                .Type<NonNullType<StringType>>();
        }

        private class PickingProductResolvers
        {
            public Task<PurchaseOrder> GetPurchaseOrder(PickingProduct product,
                PurchaseOrdersByIdBatchDataLoader purchaseOrdersBatchDataLoader, CancellationToken token)
            {
                return purchaseOrdersBatchDataLoader.LoadAsync(product.PurchaseOrderId, token);
            }
        }
    }
}