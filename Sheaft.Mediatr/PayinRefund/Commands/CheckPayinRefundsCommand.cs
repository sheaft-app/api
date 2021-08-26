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

namespace Sheaft.Mediatr.PayinRefund.Commands
{
    public class CheckPayinRefundsCommand : Command
    {
        protected CheckPayinRefundsCommand()
        {
            
        }
        [JsonConstructor]
        public CheckPayinRefundsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }

    public class CheckPayinRefundsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinRefundsCommand, Result>
    {
        public CheckPayinRefundsCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPayinRefundsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayinRefundsCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var payinRefundIds = await GetNextPayinRefundIdsAsync(skip, take, token);
            while (payinRefundIds.Any())
            {
                foreach (var payinRefundId in payinRefundIds)
                {
                    _mediatr.Post(new CheckPayinRefundCommand(request.RequestUser)
                    {
                        PayinRefundId = payinRefundId
                    });
                }

                skip += take;
                payinRefundIds = await GetNextPayinRefundIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextPayinRefundIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Refunds
                .OfType<Domain.PayinRefund>()
                .Where(c => !c.Processed && (c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created))
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}