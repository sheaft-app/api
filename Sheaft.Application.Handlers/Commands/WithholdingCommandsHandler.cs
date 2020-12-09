using Sheaft.Application.Interop;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;
using Sheaft.Exceptions;
using System.Linq;

namespace Sheaft.Application.Handlers
{
    public class WithholdingCommandsHandler : ResultsHandler,
        IRequestHandler<CreateWithholdingCommand, Result<Guid>>,
        IRequestHandler<ProcessWithholdingsCommand, Result<bool>>,
        IRequestHandler<ProcessWithholdingCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly PspOptions _pspOptions;

        public WithholdingCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<WithholdingCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateWithholdingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.UserId, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.Identifier == _pspOptions.DocumentWalletId, token);

                var withholding = new Withholding(Guid.NewGuid(), request.Amount, debitedWallet, creditedWallet);
                await _context.AddAsync(withholding, token);
                await _context.SaveChangesAsync(token);

                return Ok(withholding.Id);
            });
        }

        public async Task<Result<bool>> Handle(ProcessWithholdingsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payout = await _context.GetByIdAsync<Payout>(request.PayoutId, token);
                if (payout.Status != TransactionStatus.Succeeded)
                    return Failed<bool>(new BadRequestException(MessageKind.Withholding_Cannot_Process_Already_Succeeded));

                if(!payout.Withholdings.Any())
                    return Failed<bool>(new BadRequestException(MessageKind.Withholding_Cannot_Process_Payout_No_Withholdings));

                foreach (var withholding in payout.Withholdings)
                {
                    var result = await _mediatr.Process(new ProcessWithholdingCommand(request.RequestUser) { WithholdingId = withholding.Id }, token);
                    if (!result.Success)
                        throw result.Exception;
                }

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(ProcessWithholdingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var withholding = await _context.GetByIdAsync<Withholding>(request.WithholdingId, token);
                if (withholding.Status == TransactionStatus.Succeeded)
                    return Ok(withholding.Status);

                if (withholding.Status != TransactionStatus.Failed && withholding.Status != TransactionStatus.Waiting)
                    return Failed<TransactionStatus>(new BadRequestException(MessageKind.Withholding_Cannot_Process_Pending));

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var result = await _pspService.CreateWithholdingAsync(withholding, token);
                    if (!result.Success)
                        return Failed<TransactionStatus>(result.Exception);

                    withholding.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    withholding.SetIdentifier(result.Data.Identifier);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(withholding.Status);
                }
            });
        }
    }
}