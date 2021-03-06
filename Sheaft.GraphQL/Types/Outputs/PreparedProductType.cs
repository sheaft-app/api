using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Pickings;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PreparedProductType : SheaftOutputType<PreparedProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<PreparedProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PreparedProduct");

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<PreparedProductsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.ProductId)
                .ID(nameof(Product))
                .Name("productId");

            descriptor
                .Field(c => c.PreparedBy)
                .Name("preparedBy");

            descriptor
                .Field(c => c.PreparedOn)
                .Name("preparedOn");

            descriptor
                .Field("completed")
                .Resolve(ctx => ctx.Parent<PreparedProduct>().PreparedOn.HasValue);
            
            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");
            
            descriptor
                .Field(c => c.Conditioning)
                .Name("conditioning");
            
            descriptor
                .Field(c => c.QuantityPerUnit)
                .Name("quantityPerUnit");
            
            descriptor
                .Field(c => c.Unit)
                .Name("unit");
            
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
                .Field(c => c.ReturnableWholeSalePrice)
                .Name("returnableWholeSalePrice");

            descriptor
                .Field(c => c.ReturnableOnSalePrice)
                .Name("returnableOnSalePrice");
                
            descriptor
                .Field(c => c.ReturnableVatPrice)
                .Name("returnableVatPrice");
            
            descriptor
                .Field(c => c.ReturnableName)
                .Name("returnableName");
                
            descriptor
                .Field(c => c.ReturnableVat)
                .Name("returnableVat");
                
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
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Reference)
                .Name("reference")
                .Type<NonNullType<StringType>>();
            
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
                .ResolveWith<PreparedProductResolvers>(c => c.GetPurchaseOrder(default, default, default))
                .Type<PurchaseOrderType>();

            descriptor
                .Field("batches")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<PreparedProductResolvers>(c => c.GetBatches(default, default, default, default))
                .Type<ListType<BatchType>>();
        }

        private class PreparedProductResolvers
        {
            public Task<PurchaseOrder> GetPurchaseOrder(PreparedProduct product,
                PurchaseOrdersByIdBatchDataLoader purchaseOrdersBatchDataLoader, CancellationToken token)
            {
                return purchaseOrdersBatchDataLoader.LoadAsync(product.PurchaseOrderId, token);
            }

            public async Task<IEnumerable<Batch>> GetBatches(PreparedProduct product,
                [ScopedService] QueryDbContext context,
                BatchesByIdBatchDataLoader batchesBatchDataLoader, CancellationToken token)
            {
                var batchIds = await context.Set<PreparedProductBatch>()
                    .Where(pb => pb.PreparedProductId == product.Id)
                    .Select(pb => pb.BatchId)
                    .ToListAsync(token);
                
                return await batchesBatchDataLoader.LoadAsync(batchIds, token);
            }
        }
    }
}