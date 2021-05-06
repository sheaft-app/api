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
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.QuickOrders
{
    [ExtendObjectType(Name = "Mutation")]
    public class QuickOrderMutations : SheaftMutation
    {
        public QuickOrderMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createQuickOrder")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(QuickOrderType))]
        public async Task<QuickOrder> CreateQuickOrderAsync(
            [GraphQLType(typeof(CreateQuickOrderInputType))] [GraphQLName("input")]
            CreateQuickOrderCommand input, [Service] ISheaftMediatr mediatr,
            QuickOrdersByIdBatchDataLoader quickOrdersDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateQuickOrderCommand, Guid>(mediatr, input, token);
            return await quickOrdersDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("setQuickOrderAsDefault")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(QuickOrderType))]
        public async Task<QuickOrder> SetDefaultQuickOrderAsync(
            [GraphQLType(typeof(SetQuickOrderAsDefaultInputType))] [GraphQLName("input")]
            SetQuickOrderAsDefaultCommand input, [Service] ISheaftMediatr mediatr,
            QuickOrdersByIdBatchDataLoader quickOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await quickOrdersDataLoader.LoadAsync(input.QuickOrderId, token);
        }

        [GraphQLName("updateQuickOrder")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(QuickOrderType))]
        public async Task<QuickOrder> UpdateQuickOrderAsync(
            [GraphQLType(typeof(UpdateQuickOrderInputType))] [GraphQLName("input")]
            UpdateQuickOrderCommand input, [Service] ISheaftMediatr mediatr,
            QuickOrdersByIdBatchDataLoader quickOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await quickOrdersDataLoader.LoadAsync(input.QuickOrderId, token);
        }

        [GraphQLName("deleteQuickOrders")]
        [Authorize(Policy = Policies.STORE)]
        public async Task<bool> DeleteQuickOrdersAsync(
            [GraphQLType(typeof(DeleteQuickOrdersInputType))] [GraphQLName("input")]
            DeleteQuickOrdersCommand input,  [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}