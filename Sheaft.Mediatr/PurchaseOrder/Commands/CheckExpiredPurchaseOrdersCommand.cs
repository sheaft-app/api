using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CheckExpiredPurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public CheckExpiredPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckExpiredPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<CheckExpiredPurchaseOrdersCommand, Result>
    {
        public CheckExpiredPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckExpiredPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckExpiredPurchaseOrdersCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var purchaseOrderIds = await GetNextPurchaseOrderIdsAsync(skip, take, token);
            while (purchaseOrderIds.Any())
            {
                foreach (var purchaseOrderId in purchaseOrderIds)
                {
                    _mediatr.Post(new CheckPurchaseOrderExpirationCommand(request.RequestUser)
                    {
                        PurchaseOrderId = purchaseOrderId
                    });
                }

                skip += take;
                purchaseOrderIds = await GetNextPurchaseOrderIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextPurchaseOrderIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.PurchaseOrders
                .Get(c => c.Status == PurchaseOrderStatus.Waiting && c.Sender.Kind == ProfileKind.Consumer && (c.CreatedOn.AddDays(3) < DateTimeOffset.UtcNow || c.ExpectedDelivery.ExpectedDeliveryDate < DateTimeOffset.UtcNow), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}