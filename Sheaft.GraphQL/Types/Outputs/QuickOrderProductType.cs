using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Products;
using Sheaft.GraphQL.QuickOrders;
using Sheaft.GraphQL.Returnables;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class QuickOrderProductType : SheaftOutputType<QuickOrderProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrderProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<QuickOrderProductsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");

            descriptor
                .Field("vat")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Vat);
            
            descriptor
                .Field("unitOnSalePrice")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.OnSalePricePerUnit);
            
            descriptor
                .Field("unitWholeSalePrice")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.WholeSalePricePerUnit);

            descriptor
                .Field("unitVatPrice")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.VatPricePerUnit);

            descriptor
                .Field("unitWeight")
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Weight);

            descriptor.Field("name")
                .Type<NonNullType<StringType>>()
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Name);

            descriptor.Field("reference")
                .Type<NonNullType<StringType>>()
                .Resolve(c => c.Parent<QuickOrderProduct>().CatalogProduct.Product.Reference);

            descriptor.Field("returnable")
                .Type<ReturnableType>()
                .ResolveWith<QuickOrderProductResolvers>(c => 
                    c.GetReturnable(default, default, default));

            descriptor.Field("producer")
                .Type<NonNullType<UserType>>()
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
            
            public Task<User> GetProducer(QuickOrderProduct quickOrderProduct,
                UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                return usersDataLoader.LoadAsync(quickOrderProduct.CatalogProduct.Product.ProducerId, token);
            }
        }
    }
}
