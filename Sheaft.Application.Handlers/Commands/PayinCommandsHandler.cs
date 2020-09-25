using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Sheaft.Application.Handlers
{
    public class PayinCommandsHandler : ResultsHandler,
        IRequestHandler<CreateWebPayinCommand, Result<Guid>>,
        IRequestHandler<CheckPayinsCommand, Result<bool>>,
        IRequestHandler<CheckPayinCommand, Result<bool>>,
        IRequestHandler<RefreshPayinStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<ExpirePayinCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public PayinCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<PayinCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateWebPayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.RequestUser.Id, token);

                var webPayin = new WebPayin(Guid.NewGuid(), wallet, order);

                await _context.AddAsync(webPayin, token);
                await _context.SaveChangesAsync(token);

                var legal = await _context.GetSingleAsync<Legal>(c => c.Owner.Id == request.RequestUser.Id, token);
                var result = await _pspService.CreateWebPayinAsync(webPayin, legal.Owner, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                webPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                webPayin.SetIdentifier(result.Data.Identifier);
                webPayin.SetRedirectUrl(result.Data.RedirectUrl);
                webPayin.SetStatus(result.Data.Status);
                webPayin.SetCreditedAmount(result.Data.Credited);

                _context.Update(webPayin);

                await _context.SaveChangesAsync(token);
                return Ok(webPayin.Id);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-15);
                var payinIds = await GetNextPayinIdsAsync(expiredDate, skip, take, token);

                while (payinIds.Any())
                {
                    foreach (var payinId in payinIds)
                    {
                        await _mediatr.Post(new CheckPayinCommand(request.RequestUser)
                        {
                            PayinId = payinId
                        }, token);
                    }

                    skip += take;
                    payinIds = await GetNextPayinIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payin = await _context.GetByIdAsync<Payin>(request.PayinId, token);
                if (payin.Status == TransactionStatus.Succeeded
                    || payin.Status == TransactionStatus.Failed
                    || payin.Status == TransactionStatus.Expired)
                    return Ok(true);

                var result = await _mediatr.Process(new RefreshPayinStatusCommand(request.RequestUser, payin.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                if (payin.CreatedOn.AddMinutes(10080) > DateTimeOffset.UtcNow
                    && (result.Data == TransactionStatus.Created || result.Data == TransactionStatus.Waiting))
                    return await _mediatr.Process(new ExpirePayinCommand(request.RequestUser) { PayinId = request.PayinId }, token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpirePayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payin = await _context.GetByIdAsync<Payin>(request.PayinId, token);
                payin.SetStatus(TransactionStatus.Expired);
                _context.Update(payin);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payin = await _context.GetSingleAsync<Payin>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetPayinAsync(payin.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payin.SetStatus(pspResult.Data.Status);
                payin.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payin.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(payin);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (payin.Status)
                {
                    case TransactionStatus.Failed:
                        await _mediatr.Post(new PayinFailedEvent(request.RequestUser) { PayinId = payin.Id }, token);
                        break;
                    case TransactionStatus.Succeeded:
                        var orderResult = await _mediatr.Process(new ConfirmOrderCommand(request.RequestUser) { Id = payin.Order.Id }, token);
                        if (!orderResult.Success)
                            return Failed<TransactionStatus>(orderResult.Exception);

                        await _mediatr.Post(new PayinSucceededEvent(request.RequestUser) { PayinId = payin.Id }, token);
                        break;
                }

                return Ok(payin.Status);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayinIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Payins
                                .Get(c => (c.Status == TransactionStatus.Waiting && c.CreatedOn < expiredDate)
                                            || (c.Status == TransactionStatus.Created && c.UpdatedOn.HasValue && c.UpdatedOn.Value < expiredDate), true)
                                .OrderBy(c => c.CreatedOn)
                                .Select(c => c.Id)
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync(token);
        }
    }
}
