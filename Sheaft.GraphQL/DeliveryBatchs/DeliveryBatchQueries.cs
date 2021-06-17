using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.DeliveryBatchs
{
    [ExtendObjectType(Name = "Query")]
    public class DeliveryBatchQueries : SheaftQuery
    {
        private readonly IDeliveryBatchService _deliveryBatchService;

        public DeliveryBatchQueries(
            IDeliveryBatchService deliveryBatchService,
            ICurrentUserService currentUserService, 
            IHttpContextAccessor httpContextAccessor) 
            : base(currentUserService, httpContextAccessor)
        {
            _deliveryBatchService = deliveryBatchService;
        }
        
        [GraphQLName("availableDeliveryBatches")]
        [GraphQLType(typeof(ListType<AvailableDeliveryBatchDtoType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<IEnumerable<AvailableDeliveryBatchDto>> GetAvailableDeliveryBatches(bool includeProcessingPurchaseOrders, [ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();
            return await _deliveryBatchService.GetAvailableDeliveryBatchesAsync(CurrentUser.Id, includeProcessingPurchaseOrders, token);
        }
    }
}