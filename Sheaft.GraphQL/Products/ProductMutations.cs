using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Products
{
    [ExtendObjectType(Name = "Mutation")]
    public class ProductMutations : SheaftMutation
    {
        public ProductMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> CreateProductAsync(
            [GraphQLType(typeof(CreateProductInputType))] [GraphQLName("input")]
            CreateProductCommand input, [Service] ISheaftMediatr mediatr,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateProductCommand, Guid>(mediatr, input, token);
            return await productsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> UpdateProductAsync(
            [GraphQLType(typeof(UpdateProductInputType))] [GraphQLName("input")]
            UpdateProductCommand input, [Service] ISheaftMediatr mediatr,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await productsDataLoader.LoadAsync(input.ProductId, token);
        }

        [GraphQLName("rateProduct")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> RateProductAsync([GraphQLType(typeof(RateProductInputType))] [GraphQLName("input")]
            RateProductCommand input, [Service] ISheaftMediatr mediatr,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await productsDataLoader.LoadAsync(input.ProductId, token);
        }

        [GraphQLName("updateProductPicture")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> UpdateProductPictureAsync(
            [GraphQLType(typeof(UpdateProductPictureInputType))] [GraphQLName("input")]
            UpdateProductPreviewCommand input, [Service] ISheaftMediatr mediatr,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync<UpdateProductPreviewCommand, string>(mediatr, input, token);
            return await productsDataLoader.LoadAsync(input.ProductId, token);
        }

        [GraphQLName("setProductsAvailability")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<ProductType>))]
        public async Task<IEnumerable<Product>> SetProductsAvailabilityAsync(
            [GraphQLType(typeof(SetProductsAvailabilityInputType))] [GraphQLName("input")]
            SetProductsAvailabilityCommand input, [Service] ISheaftMediatr mediatr,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await productsDataLoader.LoadAsync(input.ProductIds.ToList(), token);
        }

        [GraphQLName("deleteProducts")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteProductsAsync(
            [GraphQLType(typeof(DeleteProductsInputType))] [GraphQLName("input")]
            DeleteProductsCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("addPictureToProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> AddPictureToProductAsync(
            [GraphQLType(typeof(AddPictureToProductInputType))] [GraphQLName("input")]
            AddPictureToProductCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("removeProductPictures")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> RemoveProductPicturesAsync(
            [GraphQLType(typeof(RemoveProductPicturesInputType))] [GraphQLName("input")]
            RemoveProductPicturesCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}