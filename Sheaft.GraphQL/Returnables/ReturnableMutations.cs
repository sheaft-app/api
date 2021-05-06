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
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Returnables
{
    [ExtendObjectType(Name = "Mutation")]
    public class ReturnableMutations : SheaftMutation
    {
        public ReturnableMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createReturnable")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ReturnableType))]
        public async Task<Returnable> CreateReturnableAsync(
            [GraphQLType(typeof(CreateReturnableInputType))] [GraphQLName("input")]
            CreateReturnableCommand input, [Service] ISheaftMediatr mediatr,
            ReturnablesByIdBatchDataLoader returnablesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateReturnableCommand, Guid>(mediatr, input, token);
            return await returnablesDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateReturnable")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ReturnableType))]
        public async Task<Returnable> UpdateReturnableAsync(
            [GraphQLType(typeof(UpdateReturnableInputType))] [GraphQLName("input")]
            UpdateReturnableCommand input, [Service] ISheaftMediatr mediatr,
            ReturnablesByIdBatchDataLoader returnablesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await returnablesDataLoader.LoadAsync(input.ReturnableId, token);
        }

        [GraphQLName("deleteReturnable")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteReturnableAsync(
            [GraphQLType(typeof(DeleteReturnableInputType))] [GraphQLName("input")]
            DeleteReturnableCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}