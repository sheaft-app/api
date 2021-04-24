using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Donation.Commands;
using Sheaft.Mediatr.Withholding.Commands;

namespace Sheaft.Mediatr.Transfer.Commands
{
    public class RefreshTransferStatusCommand : Command
    {
        protected RefreshTransferStatusCommand()
        {
            
        }
        [JsonConstructor]
        public RefreshTransferStatusCommand(RequestUser requestUser, string identifier) : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshTransferStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshTransferStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshTransferStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<RefreshTransferStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshTransferStatusCommand request,
            CancellationToken token)
        {
            var transfer =
                await _context.Transfers.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);

            if (transfer == null)
            {
                var donation =
                    await _context.Donations.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
                if (donation != null)
                {
                    _mediatr.Post(new RefreshDonationStatusCommand(request.RequestUser, request.Identifier));
                    return Success();
                }

                var withholding =
                    await _context.Withholdings.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
                if (withholding != null)
                {
                    _mediatr.Post(new RefreshWithholdingStatusCommand(request.RequestUser, request.Identifier));
                    return Success();
                }

                return Failure(MessageKind.NotFound);
            }
            
            if (transfer.Processed)
                return Success();

            var pspResult = await _pspService.GetTransferAsync(transfer.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            transfer.SetStatus(pspResult.Data.Status);
            transfer.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            transfer.SetExecutedOn(pspResult.Data.ProcessedOn);
            transfer.SetAsProcessed();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}