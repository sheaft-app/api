using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Producers;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderProductType : SheaftOutputType<OrderProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderProduct> descriptor)
        {
            base.Configure(descriptor);
            
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
                .Field(c => c.Producer)
                .Name("producer")
                .ResolveWith<OrderProductResolvers>(c=> c.GetProducer(default, default, default))
                .Type<NonNullType<ProducerType>>();

            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));
        }

        private class OrderProductResolvers
        {
            public Task<Producer> GetProducer(OrderProduct orderProduct, ProducersByIdBatchDataLoader producersDataLoader,
                CancellationToken token)
            {
                return producersDataLoader.LoadAsync(orderProduct.ProducerId, token);
            }
        }
    }
}