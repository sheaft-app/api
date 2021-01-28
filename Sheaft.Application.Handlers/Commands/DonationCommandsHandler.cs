﻿using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using System;
using Sheaft.Domain.Enums;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Application.Events;
using Sheaft.Exceptions;

namespace Sheaft.Application.Handlers
{
    public class DonationCommandsHandler : ResultsHandler,
        IRequestHandler<CreateDonationCommand, Result<Guid>>,
        IRequestHandler<CheckDonationsCommand, Result<bool>>,
        IRequestHandler<CheckDonationCommand, Result<bool>>,
        IRequestHandler<RefreshDonationStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;
        private readonly PspOptions _pspOptions;

        public DonationCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<DonationCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var preAuthorization = await _context.GetSingleAsync<PreAuthorization>(c => c.Order.Id == request.OrderId && c.Status == PreAuthorizationStatus.Succeeded, token);
                if (preAuthorization == null 
                    || preAuthorization.PaymentStatus == PaymentStatus.Cancelled
                    || preAuthorization.PaymentStatus == PaymentStatus.Expired
                    || (preAuthorization.Order.Donation != null && preAuthorization.Order.Donation.Status != TransactionStatus.Failed))
                    return BadRequest<Guid>(MessageKind.Donation_CannotCreate_AlreadySucceeded);

                var orderDonations = await _context.FindAsync<Donation>(c => c.Order.Id == request.OrderId, token);
                if (orderDonations.Any(c => c.Status != TransactionStatus.Failed))
                    return BadRequest<Guid>(MessageKind.Donation_CannotCreate_PendingDonation);

                var donationAuhtorization = new PreAuthorizedDonationPayin(Guid.NewGuid(), preAuthorization, )
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.Identifier == _pspOptions.WalletId, token);
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var donation = new Donation(Guid.NewGuid(), preAuthorization, creditedWallet, preAuthorization.Order);
                    await _context.AddAsync(donation, token);
                    await _context.SaveChangesAsync(token);

                    preAuthorization.Order.SetDonation(donation);

                    var result = await _pspService.CreateDonationAsync(donation, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    donation.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    donation.SetIdentifier(result.Data.Identifier);
                    donation.SetStatus(result.Data.Status);
                    donation.SetExecutedOn(result.Data.ProcessedOn);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(donation.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(CheckDonationsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var donationIds = await GetNextDonationIdsAsync(skip, take, token);
                while (donationIds.Any())
                {
                    foreach (var donationId in donationIds)
                    {
                        _mediatr.Post(new CheckDonationCommand(request.RequestUser)
                        {
                            DonationId = donationId
                        });
                    }

                    skip += take;
                    donationIds = await GetNextDonationIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var donation = await _context.GetByIdAsync<Donation>(request.DonationId, token);
                if (donation.Status != TransactionStatus.Created && donation.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result = await _mediatr.Process(new RefreshDonationStatusCommand(request.RequestUser, donation.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshDonationStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var donation = await _context.GetSingleAsync<Donation>(c => c.Identifier == request.Identifier, token);
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
            });
        }

        private async Task<IEnumerable<Guid>> GetNextDonationIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Donations
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}