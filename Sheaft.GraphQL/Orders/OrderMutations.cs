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
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Orders
{
    [ExtendObjectType(Name = "Mutation")]
    public class OrderMutations : SheaftMutation
    {
        public OrderMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("createConsumerOrder")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(OrderType))]
        public async Task<Order> CreateOrderAsync([GraphQLName("input")] CreateConsumerOrderCommand input,
            OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateConsumerOrderCommand, Guid>(input, token);
            return await ordersDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("updateConsumerOrder")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(OrderType))]
        public async Task<Order> UpdateOrderAsync([GraphQLName("input")] UpdateConsumerOrderCommand input,
            OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await ordersDataLoader.LoadAsync(input.OrderId, token);
        }
        
        [GraphQLName("resetConsumerOrder")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(OrderType))]
        public async Task<Order> ResetOrderAsync([GraphQLName("input")] ResetOrderCommand input,
            OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await ordersDataLoader.LoadAsync(input.OrderId, token);
        }
        
        [GraphQLName("createBusinessOrder")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> CreateBusinessOrderAsync([GraphQLName("input")] CreateBusinessOrderCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(result.ToList(), token);
        }
    }
}