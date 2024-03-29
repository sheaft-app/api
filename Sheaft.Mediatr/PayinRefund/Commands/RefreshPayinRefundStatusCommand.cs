﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PayinRefund.Commands
{
    public class RefreshPayinRefundStatusCommand : Command
    {
        protected RefreshPayinRefundStatusCommand()
        {
            
        }
        [JsonConstructor]
        public RefreshPayinRefundStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshPayinRefundStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshPayinRefundStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<RefreshPayinRefundStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshPayinRefundStatusCommand request,
            CancellationToken token)
        {
            var payinRefund = await _context.Set<Domain.PayinRefund>()
                .SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            
            if (payinRefund.Processed)
                return Success();

            var pspResult = await _pspService.GetRefundAsync(payinRefund.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            payinRefund.SetStatus(pspResult.Data.Status);
            payinRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            payinRefund.SetExecutedOn(pspResult.Data.ProcessedOn);
            payinRefund.SetAsProcessed();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}