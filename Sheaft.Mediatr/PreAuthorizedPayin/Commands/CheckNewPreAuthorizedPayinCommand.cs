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
using Sheaft.Mediatr.PreAuthorizedPayin;

namespace Sheaft.Mediatr.Payout.Commands
{
    public class CheckNewPreAuthorizedPayinCommand : Command
    {
        [JsonConstructor]
        public CheckNewPreAuthorizedPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckNewPreAuthorizedPayinCommandHandler : CommandsHandler,
        IRequestHandler<CheckNewPreAuthorizedPayinCommand, Result>
    {
        public CheckNewPreAuthorizedPayinCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckNewPreAuthorizedPayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckNewPreAuthorizedPayinCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var preAuthorizationIds = await GetNextNewPreAuthorizationIdsAsync(skip, take, token);
            while (preAuthorizationIds.Any())
            {
                foreach (var preAuthorizationId in preAuthorizationIds)
                {
                    _mediatr.Post(new CreatePreAuthorizedPayinCommand(request.RequestUser)
                    {
                        PreAuthorizationId = preAuthorizationId
                    });
                }

                skip += take;
                preAuthorizationIds = await GetNextNewPreAuthorizationIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextNewPreAuthorizationIdsAsync(int skip, int take,
            CancellationToken token)
        {
            return await _context.PreAuthorizations
                .Get(pa => pa.Status == PreAuthorizationStatus.Succeeded 
                           && pa.PaymentStatus == PaymentStatus.Validated 
                           && pa.Order.Status == OrderStatus.Validated
                           && pa.Order.PurchaseOrders.Any(po => 
                               po.AcceptedOn.HasValue 
                               && !po.WithdrawnOn.HasValue 
                               && po.CreatedOn.AddDays(5) < DateTimeOffset.UtcNow 
                               && po.CreatedOn.AddDays(7) > DateTimeOffset.UtcNow) 
                           && pa.PreAuthorizedPayin == null)
                .OrderBy(c => c.CreatedOn)
                .Skip(skip)
                .Take(take)
                .Select(c => c.Id)
                .ToListAsync(token);
        }
    }
}