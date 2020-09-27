using Sheaft.Application.Interop;
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

namespace Sheaft.Application.Handlers
{
    public class DonationCommandsHandler : ResultsHandler,
        IRequestHandler<CheckNewDonationsCommand, Result<bool>>,
        IRequestHandler<CreateDonationCommand, Result<Guid>>,
        IRequestHandler<CheckDonationsCommand, Result<bool>>,
        IRequestHandler<CheckDonationCommand, Result<bool>>,
        IRequestHandler<RefreshDonationStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<ExpireDonationCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(CheckNewDonationsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var orderIds = await GetNextOrderIdsAsync(skip, take, token);
                while (orderIds.Any())
                {
                    foreach (var orderId in orderIds)
                    {
                        await _mediatr.Post(new CreateDonationCommand(request.RequestUser)
                        {
                            OrderId = orderId
                        }, token);
                    }

                    skip += take;
                    orderIds = await GetNextOrderIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(CreateDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (order.Payin == null 
                    || order.Payin.Status != TransactionStatus.Succeeded 
                    || (order.Donation != null && order.Donation.Status != TransactionStatus.Expired))
                    return Failed<Guid>(new InvalidOperationException());

                var orderDonations = await _context.FindAsync<Donation>(c => c.Order.Id == order.Id, token);
                if (orderDonations.Any(c => c.Status != TransactionStatus.Expired))
                    return Failed<Guid>(new InvalidOperationException());

                if (orderDonations.Count(c => c.Status == TransactionStatus.Expired) >= 3)
                {
                    order.SetSkipBackgroundProcessing(true);
                    _context.Update(order);
                    await _context.SaveChangesAsync(token);

                    await _mediatr.Post(new CreateDonationFailedEvent(request.RequestUser)
                    {
                        OrderId = order.Id
                    }, token);

                    return TooManyRetries<Guid>();
                }

                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.Identifier == _pspOptions.WalletId, token);
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var donation = new Donation(Guid.NewGuid(), order.Payin.CreditedWallet, creditedWallet, order);
                    await _context.AddAsync(donation, token);
                    await _context.SaveChangesAsync(token);

                    order.SetDonation(donation);

                    var result = await _pspService.CreateDonationAsync(donation, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    donation.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    donation.SetIdentifier(result.Data.Identifier);
                    donation.SetStatus(result.Data.Status);

                    _context.Update(donation);
                    _context.Update(order);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(donation.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(CheckDonationsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckDonationsFromMinutes);
                var transferIds = await GetNextDonationIdsAsync(expiredDate, skip, take, token);

                while (transferIds.Any())
                {
                    foreach (var transferId in transferIds)
                    {
                        await _mediatr.Post(new CheckTransferCommand(request.RequestUser)
                        {
                            TransferId = transferId
                        }, token);
                        break;
                    }

                    skip += take;
                    transferIds = await GetNextDonationIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var donation = await _context.GetByIdAsync<Donation>(request.DonationId, token);
                if (donation.Status != TransactionStatus.Created && donation.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (donation.CreatedOn.AddMinutes(_routineOptions.CheckDonationExpiredFromMinutes) < DateTimeOffset.UtcNow && donation.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpireDonationCommand(request.RequestUser) { DonationId = request.DonationId }, token);

                var result = await _mediatr.Process(new RefreshDonationStatusCommand(request.RequestUser, donation.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpireDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var donation = await _context.GetByIdAsync<Donation>(request.DonationId, token);
                donation.SetStatus(TransactionStatus.Expired);

                _context.Update(donation);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshDonationStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var donation = await _context.GetSingleAsync<Donation>(c => c.Identifier == request.Identifier, token);
                if (donation.Status == TransactionStatus.Succeeded || donation.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetTransferAsync(donation.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                donation.SetStatus(pspResult.Data.Status);
                donation.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                donation.SetExecutedOn(pspResult.Data.ProcessedOn);

                _context.Update(donation);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (donation.Status)
                {
                    case TransactionStatus.Failed:
                        await _mediatr.Post(new DonationFailedEvent(request.RequestUser) { DonationId = donation.Id }, token);
                        break;
                    case TransactionStatus.Succeeded:
                        await _mediatr.Post(new DonationSucceededEvent(request.RequestUser) { DonationId = donation.Id }, token);
                        break;
                }

                return Ok(donation.Status);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextOrderIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Orders
                .Get(c => c.Donate > 0
                            && c.Payin != null
                            && c.Payin.Status == TransactionStatus.Succeeded
                            && (c.Donation == null || c.Donation.Status == TransactionStatus.Expired), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }

        private async Task<IEnumerable<Guid>> GetNextDonationIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Donations
                .Get(c => c.CreatedOn < expiredDate
                      && (c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}