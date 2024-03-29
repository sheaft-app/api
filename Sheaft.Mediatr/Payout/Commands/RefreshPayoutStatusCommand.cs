﻿using System.Linq;
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
using Sheaft.Mediatr.Withholding.Commands;

namespace Sheaft.Mediatr.Payout.Commands
{
    public class RefreshPayoutStatusCommand : Command
    {
        protected RefreshPayoutStatusCommand()
        {
            
        }
        [JsonConstructor]
        public RefreshPayoutStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshPayoutStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPayoutStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshPayoutStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<RefreshPayoutStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshPayoutStatusCommand request, CancellationToken token)
        {
            var payout = await _context.Payouts.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            if (payout.Processed)
                return Success();
            
            var pspResult = await _pspService.GetPayoutAsync(payout.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);
            
            payout.SetStatus(pspResult.Data.Status);
            payout.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            payout.SetExecutedOn(pspResult.Data.ProcessedOn);
            payout.SetAsProcessed();

            await _context.SaveChangesAsync(token);

            if (payout.Status == TransactionStatus.Succeeded && payout.Withholdings.Any())
                _mediatr.Post(new ProcessWithholdingsCommand(request.RequestUser) {PayoutId = payout.Id});

            return Success();
        }
    }
}