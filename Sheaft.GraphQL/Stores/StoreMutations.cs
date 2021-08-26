using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Store.Commands;

namespace Sheaft.GraphQL.Stores
{
    [ExtendObjectType(Name = "Mutation")]
    public class StoreMutations : SheaftMutation
    {
        public StoreMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("registerStore")]
        [Authorize(Policy = Policies.AUTHENTICATED)]
        [GraphQLType(typeof(StoreType))]
        public async Task<Store> RegisterStoreAsync([GraphQLType(typeof(RegisterStoreInputType))] [GraphQLName("input")]
            RegisterStoreCommand input, [Service] ISheaftMediatr mediatr,
            StoresByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<RegisterStoreCommand, Guid>(mediatr, input, token);
            return await storesDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateStore")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(StoreType))]
        public async Task<Store> UpdateStoreAsync([GraphQLType(typeof(UpdateStoreInputType))] [GraphQLName("input")]
            UpdateStoreCommand input, [Service] ISheaftMediatr mediatr,
            StoresByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await storesDataLoader.LoadAsync(input.StoreId, token);
        }
    }
}