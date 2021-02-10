using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.PayinRefund.Commands
{
    public class CheckPayinRefundsCommand : Command
    {
        [JsonConstructor]
        public CheckPayinRefundsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }

    public class CheckPayinRefundsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinRefundsCommand, Result>
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
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}