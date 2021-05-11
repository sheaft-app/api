using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Catalogs
{
    [ExtendObjectType(Name = "Mutation")]
    public class CatalogMutations : SheaftMutation
    {
        public CatalogMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createCatalog")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> CreateCatalogAsync(
            [GraphQLType(typeof(CreateCatalogInputType))] [GraphQLName("input")]
            CreateCatalogCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateCatalogCommand, Guid>(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateCatalog")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> UpdateCatalogAsync(
            [GraphQLType(typeof(UpdateCatalogInputType))] [GraphQLName("input")]
            UpdateCatalogCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.CatalogId, token);
        }

        [GraphQLName("deleteCatalogs")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteCatalogsAsync(
            [GraphQLType(typeof(DeleteCatalogsInputType))] [GraphQLName("input")]
            DeleteCatalogsCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("addOrUpdateProductsToCatalog")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> AddOrUpdateProductsToCatalogAsync(
            [GraphQLType(typeof(AddOrUpdateProductsToCatalogInputType))] [GraphQLName("input")]
            AddOrUpdateProductsToCatalogCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.CatalogId, token);
        }

        [GraphQLName("removeProductsFromCatalog")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> RemoveProductsFromCatalogAsync(
            [GraphQLType(typeof(RemoveProductsFromCatalogInputType))] [GraphQLName("input")]
            RemoveProductsFromCatalogCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.CatalogId, token);
        }

        [GraphQLName("cloneCatalog")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> CloneCatalogAsync([GraphQLType(typeof(CloneCatalogInputType))] [GraphQLName("input")]
            CloneCatalogCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CloneCatalogCommand, Guid>(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateCatalogPrices")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> UpdateAllCatalogPricesAsync(
            [GraphQLType(typeof(UpdateAllCatalogPricesInputType))] [GraphQLName("input")]
            UpdateAllCatalogPricesCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.CatalogId, token);
        }

        [GraphQLName("setCatalogAsDefault")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(CatalogType))]
        public async Task<Catalog> SetCatalogAsDefaultAsync(
            [GraphQLType(typeof(SetCatalogAsDefaultInputType))] [GraphQLName("input")]
            SetCatalogAsDefaultCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.CatalogId, token);
        }

        [GraphQLName("setCatalogsAvailability")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<CatalogType>))]
        public async Task<IEnumerable<Catalog>> SetCatalogsAvailabilityAsync(
            [GraphQLType(typeof(SetCatalogsAvailabilityInputType))] [GraphQLName("input")]
            SetCatalogsAvailabilityCommand input, [Service] ISheaftMediatr mediatr,
            CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.CatalogIds.ToList(), token);
        }
    }
}