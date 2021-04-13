using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Payin;
using Sheaft.Mediatr.Donation.Commands;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class RefreshPayinStatusCommand : Command
    {
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
            var payin = await _context.GetSingleAsync<Domain.Payin>(c => c.Identifier == request.Identifier, token);
            if (payin.Status == TransactionStatus.Succeeded 
                || payin.Status == TransactionStatus.Failed 
                || payin.Status == TransactionStatus.Cancelled)
                return Success();

            var pspResult = await _pspService.GetPayinAsync(payin.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult.Exception);

            payin.SetStatus(pspResult.Data.Status);
            payin.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            payin.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);

            //TO REMOVE 1 MONTH AFTER RELEASE
            if (payin.Kind == TransactionKind.WebPayin)
            {
                switch (payin.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new FailOrderCommand(request.RequestUser)
                            {OrderId = payin.Order.Id});
                        break;
                    case TransactionStatus.Succeeded:
                        _mediatr.Post(new ConfirmOrderCommand(request.RequestUser) {OrderId = payin.Order.Id});
                        break;
                }
                
                if(payin.Order.Donation > 0)
                    _mediatr.Schedule(new CreateDonationCommand(request.RequestUser){OrderId = payin.Order.Id}, TimeSpan.FromDays(1));
            }

            return Success();
        }
    }
}