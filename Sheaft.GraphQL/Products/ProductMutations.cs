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
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> CreateProductAsync(
            [GraphQLType(typeof(CreateProductInputType))] [GraphQLName("input")]
            CreateProductCommand input,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateProductCommand, Guid>(input, token);
            return await productsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> UpdateProductAsync(
            [GraphQLType(typeof(UpdateProductInputType))] [GraphQLName("input")]
            UpdateProductCommand input,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await productsDataLoader.LoadAsync(input.ProductId, token);
        }

        [GraphQLName("rateProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> RateProductAsync([GraphQLType(typeof(RateProductInputType))] [GraphQLName("input")]
            RateProductCommand input,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await productsDataLoader.LoadAsync(input.ProductId, token);
        }

        [GraphQLName("updateProductPicture")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProductType))]
        public async Task<Product> UpdateProductPictureAsync(
            [GraphQLType(typeof(UpdateProductPictureInputType))] [GraphQLName("input")]
            UpdateProductPreviewCommand input,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync<UpdateProductPreviewCommand, string>(input, token);
            return await productsDataLoader.LoadAsync(input.ProductId, token);
        }

        [GraphQLName("setProductsAvailability")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<ProductType>))]
        public async Task<IEnumerable<Product>> SetProductsAvailabilityAsync(
            [GraphQLType(typeof(SetProductsAvailabilityInputType))] [GraphQLName("input")]
            SetProductsAvailabilityCommand input,
            ProductsByIdBatchDataLoader productsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await productsDataLoader.LoadAsync(input.ProductIds.ToList(), token);
        }

        [GraphQLName("deleteProducts")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteProductsAsync(
            [GraphQLType(typeof(DeleteProductsInputType))] [GraphQLName("input")]
            DeleteProductsCommand input,
            CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }

        [GraphQLName("addPictureToProduct")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> AddPictureToProductAsync(
            [GraphQLType(typeof(AddPictureToProductInputType))] [GraphQLName("input")]
            AddPictureToProductCommand input,
            CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }

        [GraphQLName("removeProductPictures")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> RemoveProductPicturesAsync(
            [GraphQLType(typeof(RemoveProductPicturesInputType))] [GraphQLName("input")]
            RemoveProductPicturesCommand input,
            CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}