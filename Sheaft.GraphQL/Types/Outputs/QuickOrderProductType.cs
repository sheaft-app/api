﻿using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Returnables;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class QuickOrderProductType : SheaftOutputType<QuickOrderProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrderProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field("id")
                .ID(nameof(Product))
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.ProductId);
            
            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");

            descriptor
                .Field("quantityPerUnit")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.QuantityPerUnit);
                
            descriptor
                .Field("available")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Available);
            
            descriptor
                .Field("weight")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Weight);
            
            descriptor
                .Field("conditioning")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Conditioning);
                
            descriptor
                .Field("unit")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Unit);
            
            descriptor
                .Field("vat")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Vat);
            
            descriptor
                .Field("onSalePricePerUnit")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.OnSalePricePerUnit);
            
            descriptor
                .Field("wholeSalePricePerUnit")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.WholeSalePricePerUnit);

            descriptor
                .Field("vatPricePerUnit")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.VatPricePerUnit);
            
            descriptor
                .Field("onSalePrice")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.OnSalePrice);
            
            descriptor
                .Field("wholeSalePrice")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.WholeSalePrice);

            descriptor
                .Field("vatPrice")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.VatPrice);

            descriptor
                .Field("unitWeight")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Weight);

            descriptor.Field("name")
                .Type<NonNullType<StringType>>()
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Name);

            descriptor.Field("reference")
                .Type<NonNullType<StringType>>()
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Reference);

            descriptor
                .Field("isReturnable")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.ReturnableId.HasValue);
            
            descriptor.Field("returnable")
                .Type<ReturnableType>()
                .ResolveWith<QuickOrderProductResolvers>(c => 
                    c.GetReturnable(default, default, default));

            descriptor.Field("producer")
                .Type<NonNullType<ProducerType>>()
                .ResolveWith<QuickOrderProductResolvers>(c => 
                    c.GetProducer(default, default, default));
        }

        private class QuickOrderProductResolvers
        {
            public Task<Returnable> GetReturnable(QuickOrderProduct quickOrderProduct,
                ReturnablesByIdBatchDataLoader returnablesDataLoader, CancellationToken token)
            {
                if (!quickOrderProduct.CatalogProduct.Product.ReturnableId.HasValue)
                    return null;
                
                return returnablesDataLoader.LoadAsync(quickOrderProduct.CatalogProduct.Product.ReturnableId.Value, token);
            }
            
            public Task<Producer> GetProducer(QuickOrderProduct quickOrderProduct,
                ProducersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(quickOrderProduct.CatalogProduct.Product.ProducerId, token);
            }
        }
    }
}
