using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Products;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CatalogProductType : SheaftOutputType<CatalogProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<CatalogProductsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.VatPricePerUnit)
                .Name("vatPricePerUnit");
                
            descriptor
                .Field(c => c.OnSalePricePerUnit)
                .Name("onSalePricePerUnit");
                
            descriptor
                .Field(c => c.WholeSalePricePerUnit)
                .Name("wholeSalePricePerUnit");
                
            descriptor
                .Field(c => c.OnSalePrice)
                .Name("onSalePrice");
                
            descriptor
                .Field(c => c.WholeSalePrice)
                .Name("wholeSalePrice");
                
            descriptor
                .Field(c => c.VatPrice)
                .Name("vatPrice");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("addedOn");

            descriptor
                .Field(c => c.Catalog)
                .Name("catalog")
                .ResolveWith<CatalogProductResolvers>(c => c.GetCatalog(default, default, default))
                .Type<NonNullType<CatalogType>>();

            descriptor
                .Field(c => c.Product)
                .Name("product")
                .ResolveWith<CatalogProductResolvers>(c => c.GetProduct(default, default, default))
                .Type<NonNullType<ProductType>>();
        }

        private class CatalogProductResolvers
        {
            public Task<Catalog> GetCatalog(CatalogProduct catalogProduct,
                CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
            {
                return catalogsDataLoader.LoadAsync(catalogProduct.CatalogId, token);
            }
            
            public Task<Product> GetProduct(CatalogProduct catalogProduct,
                ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
            {
                return productsDataLoader.LoadAsync(catalogProduct.ProductId, token);
            }
        }
    }
}