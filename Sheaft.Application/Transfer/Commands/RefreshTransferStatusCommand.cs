using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RefreshTransferStatusCommand : Command<TransactionStatus>
    {
        [JsonConstructor]
        public RefreshTransferStatusCommand(RequestUser requestUser, string identifier) : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
    
    public class RefreshTransferStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshTransferStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public RefreshTransferStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<RefreshTransferStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshTransferStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var transfer = await _context.FindSingleAsync<Transfer>(c => c.Identifier == request.Identifier, token);
                if(transfer != null)
                    return await HandleTransferStatusAsync(request, transfer, token);

                var donation = await _context.FindSingleAsync<Donation>(c => c.Identifier == request.Identifier, token);
                if(donation != null)
                    return await HandleDonationStatusAsync(request, donation, token);

                var withholding = await _context.FindSingleAsync<Withholding>(c => c.Identifier == request.Identifier, token);
                if (withholding != null)
                    return await HandleWithholdingStatusAsync(request, withholding, token);

                return NotFound<TransactionStatus>();
            });
        }

        private async Task<Result<TransactionStatus>> HandleTransferStatusAsync(RefreshTransferStatusCommand request, Transfer transfer, CancellationToken token)
        {
            if (transfer.Status == TransactionStatus.Succeeded || transfer.Status == TransactionStatus.Failed)
                return Ok(transfer.Status);

            var pspResult = await _pspService.GetTransferAsync(transfer.Identifier, token);
            if (!pspResult.Success)
                return Failed<TransactionStatus>(pspResult.Exception);

            transfer.SetStatus(pspResult.Data.Status);
            transfer.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            transfer.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);

            switch (transfer.Status)
            {
                case TransactionStatus.Failed:
                    _mediatr.Post(new TransferFailedEvent(request.RequestUser) { TransferId = transfer.Id });
                    break;
            }

            return Ok(transfer.Status);
        }

        private async Task<Result<TransactionStatus>> HandleDonationStatusAsync(RefreshTransferStatusCommand request, Donation donation, CancellationToken token)
        {
            if (donation.Status == TransactionStatus.Succeeded || donation.Status == TransactionStatus.Failed)
                return Ok(donation.Status);

            var pspResult = await _pspService.GetTransferAsync(donation.Identifier, token);
            if (!pspResult.Success)
                return Failed<TransactionStatus>(pspResult.Exception);

            donation.SetStatus(pspResult.Data.Status);
            donation.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            donation.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);

            switch (donation.Status)
            {
                case TransactionStatus.Failed:
                    _mediatr.Post(new DonationFailedEvent(request.RequestUser) { DonationId = donation.Id });
                    break;
            }

            return Ok(donation.Status);
        }

        private async Task<Result<TransactionStatus>> HandleWithholdingStatusAsync(RefreshTransferStatusCommand request, Withholding withholding, CancellationToken token)
        {
            if (withholding.Status == TransactionStatus.Succeeded || withholding.Status == TransactionStatus.Failed)
                return Ok(withholding.Status);

            var pspResult = await _pspService.GetTransferAsync(withholding.Identifier, token);
            if (!pspResult.Success)
                return Failed<TransactionStatus>(pspResult.Exception);

            withholding.SetStatus(pspResult.Data.Status);
            withholding.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            withholding.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);

            switch (withholding.Status)
            {
                case TransactionStatus.Failed:
                    _mediatr.Post(new WithholdingFailedEvent(request.RequestUser) { WithholdingId = withholding.Id });
                    break;
            }

            return Ok(withholding.Status);
        }

        private async Task<IEnumerable<Guid>> GetNextTransferIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Transfers
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
