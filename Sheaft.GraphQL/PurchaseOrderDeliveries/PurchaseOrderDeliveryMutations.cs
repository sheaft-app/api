using System;
using System.Collections.Generic;
using System.IO;
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
using Sheaft.GraphQL.Jobs;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.PurchaseOrderDelivery.Commands;

namespace Sheaft.GraphQL.PurchaseOrderDeliveries
{
    [ExtendObjectType(Name = "Mutation")]
    public class PurchaseOrderDeliveryMutations : SheaftMutation
    {
        public PurchaseOrderDeliveryMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("startDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PurchaseOrderDeliveryType))]
        public async Task<PurchaseOrderDelivery> StartDelivery(
            [GraphQLType(typeof(StartPurchaseOrderDeliveryInputType))] [GraphQLName("input")]
            StartPurchaseOrderDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            PurchaseOrderDeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PurchaseOrderDeliveryId, token);
        }

        [GraphQLName("completeDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PurchaseOrderDeliveryType))]
        public async Task<PurchaseOrderDelivery> CompleteDelivery(
            [GraphQLType(typeof(CompletePurchaseOrderDeliveryInputType))] [GraphQLName("input")]
            CompletePurchaseOrderDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            PurchaseOrderDeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PurchaseOrderDeliveryId, token);
        }

        [GraphQLName("skipDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PurchaseOrderDeliveryType))]
        public async Task<PurchaseOrderDelivery> SkipDelivery(
            [GraphQLType(typeof(SkipPurchaseOrderDeliveryInputType))] [GraphQLName("input")]
            SkipPurchaseOrderDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            PurchaseOrderDeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PurchaseOrderDeliveryId, token);
        }
    }
}