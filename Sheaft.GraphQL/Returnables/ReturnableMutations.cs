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
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Returnables
{
    [ExtendObjectType(Name = "Mutation")]
    public class ReturnableMutations : SheaftMutation
    {
        public ReturnableMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("createReturnable")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ReturnableType))]
        public async Task<Returnable> CreateReturnableAsync([GraphQLName("input")] CreateReturnableCommand input,
            ReturnablesByIdBatchDataLoader returnablesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateReturnableCommand, Guid>(input, token);
            return await returnablesDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("udpateReturnable")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ReturnableType))]
        public async Task<Returnable> UpdateReturnableAsync([GraphQLName("input")] UpdateReturnableCommand input,
            ReturnablesByIdBatchDataLoader returnablesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await returnablesDataLoader.LoadAsync(input.ReturnableId, token);
        }
        
        [GraphQLName("deleteReturnable")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteReturnableAsync([GraphQLName("input")] DeleteReturnableCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}