using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckPayinRefundsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinRefundsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }

    public class CheckPayinRefundsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinRefundsCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CheckPayinRefundsCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckPayinRefundsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckPayinRefundsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
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

                return Ok(true);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayinRefundIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Refunds
                .OfType<PayinRefund>()
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
