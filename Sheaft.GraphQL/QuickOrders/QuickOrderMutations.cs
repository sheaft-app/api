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
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.QuickOrders
{
    [ExtendObjectType(Name = "Mutation")]
    public class QuickOrderMutations : SheaftMutation
    {
        public QuickOrderMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("createQuickOrder")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(QuickOrderType))]
        public async Task<QuickOrder> CreateQuickOrderAsync([GraphQLName("input")] CreateQuickOrderCommand input,
            QuickOrdersByIdBatchDataLoader quickOrdersDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateQuickOrderCommand, Guid>(input, token);
            return await quickOrdersDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("setQuickOrderAsDefault")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(QuickOrderType))]
        public async Task<QuickOrder> SetDefaultQuickOrderAsync([GraphQLName("input")] SetQuickOrderAsDefaultCommand input,
            QuickOrdersByIdBatchDataLoader quickOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await quickOrdersDataLoader.LoadAsync(input.QuickOrderId, token);
        }
        
        [GraphQLName("updateQuickOrder")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(QuickOrderType))]
        public async Task<QuickOrder> UpdateQuickOrderAsync([GraphQLName("input")] UpdateQuickOrderCommand input,
            QuickOrdersByIdBatchDataLoader quickOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await quickOrdersDataLoader.LoadAsync(input.QuickOrderId, token);
        }
        
        [GraphQLName("deleteQuickOrders")]
        [Authorize(Policy = Policies.STORE)]
        public async Task<bool> DeleteQuickOrdersAsync([GraphQLName("input")] DeleteQuickOrdersCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}