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
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.PurchaseOrders
{
    [ExtendObjectType(Name = "Mutation")]
    public class PurchaseOrderMutations : SheaftMutation
    {
        public PurchaseOrderMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
       
        [GraphQLName("acceptPurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> AcceptPurchaseOrdersAsync([GraphQLName("input")] AcceptPurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("shipPurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> ShipPurchaseOrdersAsync([GraphQLName("input")] ShipPurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("deliverPurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> DeliverPurchaseOrdersAsync([GraphQLName("input")] DeliverPurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("processPurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> ProcessPurchaseOrdersAsync([GraphQLName("input")] ProcessPurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("completePurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> CompletePurchaseOrdersAsync([GraphQLName("input")] CompletePurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("cancelPurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> CancelPurchaseOrdersAsync([GraphQLName("input")] CancelPurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("withdrawnPurchaseOrders")]
        [Authorize(Policy = Policies.STORE_OR_CONSUMER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> WithdrawnPurchaseOrdersAsync([GraphQLName("input")] WithdrawnPurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("refusePurchaseOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        public async Task<IEnumerable<PurchaseOrder>> RefusePurchaseOrdersAsync([GraphQLName("input")] RefusePurchaseOrdersCommand input,
            PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await purchaseOrdersDataLoader.LoadAsync(input.PurchaseOrderIds.ToList(), token);
        }
        
        [GraphQLName("deleteConsumerOrder")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeletePurchaseOrdersAsync([GraphQLName("input")] DeletePurchaseOrdersCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}