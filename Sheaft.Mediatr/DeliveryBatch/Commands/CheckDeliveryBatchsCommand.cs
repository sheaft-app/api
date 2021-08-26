using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.DeliveryBatch;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class CheckDeliveryBatchsCommand : Command
    {
        protected CheckDeliveryBatchsCommand()
        {
            
        }
        [JsonConstructor]
        public CheckDeliveryBatchsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckDeliveryBatchsCommandHandler : CommandsHandler,
        IRequestHandler<CheckDeliveryBatchsCommand, Result>
    {
        public CheckDeliveryBatchsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckDeliveryBatchsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckDeliveryBatchsCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var nextDeliveryBatchsAsync = await GetNextDeliveryBatchIdsAsync(skip, take, token);
            while (nextDeliveryBatchsAsync.Any())
            {
                foreach (var deliveryBatch in nextDeliveryBatchsAsync)
                {
                    if((DateTimeOffset.UtcNow - deliveryBatch.Value).Days % 2 == 0)
                        _mediatr.Post(new DeliveryBatchPendingEvent(deliveryBatch.Key));
                }

                skip += take;
                nextDeliveryBatchsAsync = await GetNextDeliveryBatchIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<KeyValuePair<Guid, DateTimeOffset>>> GetNextDeliveryBatchIdsAsync(int skip, int take, CancellationToken token)
        {
            var day = DateTimeOffset.UtcNow.AddDays(-2);
            return await _context.DeliveryBatches
                .Where(c => c.ScheduledOn < day && c.Status != DeliveryBatchStatus.Cancelled && c.Status != DeliveryBatchStatus.Completed && c.Status != DeliveryBatchStatus.Partial)
                .OrderBy(c => c.CreatedOn)
                .Select(c => new KeyValuePair<Guid,DateTimeOffset>(c.Id, c.ScheduledOn))
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}