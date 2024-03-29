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

namespace Sheaft.Mediatr.Payin.Commands
{
    public class RefreshPayinStatusCommand : Command
    {
        protected RefreshPayinStatusCommand()
        {
            
        }
        [JsonConstructor]
        public RefreshPayinStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshPayinStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPayinStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshPayinStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<RefreshPayinStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshPayinStatusCommand request, CancellationToken token)
        {
            var payin = await _context.Payins.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            if (payin.Processed)
                return Success();

            var pspResult = await _pspService.GetPayinAsync(payin.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            payin.SetStatus(pspResult.Data.Status);
            payin.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            payin.SetExecutedOn(pspResult.Data.ProcessedOn);
            payin.SetAsProcessed();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}