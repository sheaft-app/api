using System;
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
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Store.Commands;

namespace Sheaft.GraphQL.Stores
{
    [ExtendObjectType(Name = "Mutation")]
    public class StoreMutations : SheaftMutation
    {
        public StoreMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("registerStore")]
        [Authorize(Policy = Policies.UNREGISTERED)]
        [GraphQLType(typeof(StoreType))]
        public async Task<Store> RegisterStoreAsync([GraphQLName("input")] RegisterStoreCommand input,
            StoresByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<RegisterStoreCommand, Guid>(input, token);
            return await storesDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("updateStore")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(StoreType))]
        public async Task<Store> UpdateStoreAsync([GraphQLName("input")] UpdateStoreCommand input,
            StoresByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await storesDataLoader.LoadAsync(input.StoreId, token);
        }
    }
}