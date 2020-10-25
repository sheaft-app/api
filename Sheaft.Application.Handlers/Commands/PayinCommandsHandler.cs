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
using Sheaft.Exceptions;
using Microsoft.Extensions.Options;
using Sheaft.Options;

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
        private readonly RoutineOptions _routineOptions;

        public PayinCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<PayinCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateWebPayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if(order.Status == OrderStatus.Validated)
                    return Failed<Guid>(new ValidationException());

                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.RequestUser.Id, token);

                if (order.TotalOnSalePrice < 1)
                    return Failed<Guid>(new ValidationException(MessageKind.Order_Total_CannotBe_LowerThan, 1));

                var webPayin = new WebPayin(Guid.NewGuid(), wallet, order);

                await _context.AddAsync(webPayin, token);
                await _context.SaveChangesAsync(token);

                order.SetPayin(webPayin);

                var legal = await _context.GetSingleAsync<Legal>(c => c.Owner.Id == request.RequestUser.Id, token);
                var result = await _pspService.CreateWebPayinAsync(webPayin, legal.Owner, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                webPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                webPayin.SetIdentifier(result.Data.Identifier);
                webPayin.SetRedirectUrl(result.Data.RedirectUrl);
                webPayin.SetStatus(result.Data.Status);
                webPayin.SetCreditedAmount(result.Data.Credited);

                await _context.SaveChangesAsync(token);
                return Ok(webPayin.Id);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var payinIds = await GetNextPayinIdsAsync(skip, take, token);
                while (payinIds.Any())
                {
                    foreach (var payinId in payinIds)
                    {
                        _mediatr.Post(new CheckPayinCommand(request.RequestUser)
                        {
                            PayinId = payinId
                        });
                    }

                    skip += take;
                    payinIds = await GetNextPayinIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetByIdAsync<Payin>(request.PayinId, token);
                if (payin.Status != TransactionStatus.Created && payin.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (payin.CreatedOn.AddMinutes(_routineOptions.CheckPayinExpiredFromMinutes) < DateTimeOffset.UtcNow && payin.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpirePayinCommand(request.RequestUser) { PayinId = request.PayinId }, token);

                var result = await _mediatr.Process(new RefreshPayinStatusCommand(request.RequestUser, payin.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpirePayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetByIdAsync<Payin>(request.PayinId, token);
                payin.SetStatus(TransactionStatus.Expired);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetSingleAsync<Payin>(c => c.Identifier == request.Identifier, token);
                if (payin.Status == TransactionStatus.Succeeded || payin.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetPayinAsync(payin.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payin.SetStatus(pspResult.Data.Status);
                payin.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payin.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (payin.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new FailOrderCommand(request.RequestUser) { OrderId = payin.Order.Id, PayinId = payin.Id });
                        break;
                    case TransactionStatus.Succeeded:
                        _mediatr.Post(new ConfirmOrderCommand(request.RequestUser) { OrderId = payin.Order.Id });
                        _mediatr.Post(new PayinSucceededEvent(request.RequestUser) { PayinId = payin.Id });
                        break;
                }

                return Ok(payin.Status);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayinIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Payins
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
