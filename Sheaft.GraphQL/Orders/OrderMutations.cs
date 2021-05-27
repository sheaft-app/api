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
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Orders
{
    [ExtendObjectType(Name = "Mutation")]
    public class OrderMutations : SheaftMutation
    {
        public OrderMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createConsumerOrder")]
        [GraphQLType(typeof(OrderType))]
        public async Task<Order> CreateOrderAsync(
            [GraphQLType(typeof(CreateConsumerOrderInputType))] [GraphQLName("input")]
            CreateConsumerOrderCommand input, [Service] ISheaftMediatr mediatr,
            OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateConsumerOrderCommand, Guid>(mediatr, input, token);
            return await ordersDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateConsumerOrder")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(OrderType))]
        public async Task<Order> UpdateOrderAsync(
            [GraphQLType(typeof(UpdateConsumerOrderInputType))] [GraphQLName("input")]
            UpdateConsumerOrderCommand input, [Service] ISheaftMediatr mediatr,
            OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await ordersDataLoader.LoadAsync(input.OrderId, token);
        }

        [GraphQLName("resetConsumerOrder")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(OrderType))]
        public async Task<Order> ResetOrderAsync([GraphQLType(typeof(ResetOrderInputType))] [GraphQLName("input")]
            ResetOrderCommand input, [Service] ISheaftMediatr mediatr,
            OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await ordersDataLoader.LoadAsync(input.OrderId, token);
        }

        [GraphQLName("createBusinessOrder")]
        [Authorize(Policy = Policies.STORE)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> CreateBusinessOrderAsync(
            [GraphQLType(typeof(CreateBusinessOrderInputType))] [GraphQLName("input")]
            CreateBusinessOrderCommand input, [Service] ISheaftMediatr mediatr,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<CreateBusinessOrderCommand, IEnumerable<Guid>>(mediatr, input, token);
            return await purchaseOrdersDataLoader.LoadAsync(result.ToList(), token);
        }
    }
}