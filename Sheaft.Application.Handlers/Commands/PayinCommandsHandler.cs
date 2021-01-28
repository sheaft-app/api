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
        IRequestHandler<CreatePreAuthorizedPayinCommand, Result<Guid>>,
        IRequestHandler<CheckPayinsCommand, Result<bool>>,
        IRequestHandler<CheckPayinCommand, Result<bool>>,
        IRequestHandler<RefreshPayinStatusCommand, Result<TransactionStatus>>
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

        public async Task<Result<Guid>> Handle(CreatePreAuthorizedPayinCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var preAuthorization = await _context.GetByIdAsync<PreAuthorization>(request.PreAuthorizationId, token);
                if(preAuthorization.Order.Status == OrderStatus.Validated)
                    return BadRequest<Guid>(MessageKind.Payin_CannotCreate_Order_Already_Validated);
                    

                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.RequestUser.Id, token);

                if (preAuthorization.Order.TotalOnSalePrice < 1)
                    return BadRequest<Guid>(MessageKind.Order_Total_CannotBe_LowerThan, 1);

                var preAuthorizedPayin = new PreAuthorizedPayin(Guid.NewGuid(), request.PurchaseOrderId, preAuthorization, wallet);

                await _context.AddAsync(preAuthorizedPayin, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePreAuthorizedPayinAsync(preAuthorizedPayin, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                preAuthorizedPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                preAuthorizedPayin.SetIdentifier(result.Data.Identifier);
                preAuthorizedPayin.SetStatus(result.Data.Status);
                preAuthorizedPayin.SetCreditedAmount(result.Data.Credited);
                preAuthorizedPayin.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                return Ok(preAuthorizedPayin.Id);
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

                var result = await _mediatr.Process(new RefreshPayinStatusCommand(request.RequestUser, payin.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetSingleAsync<Payin>(c => c.Identifier == request.Identifier, token);
                if (payin.Status == TransactionStatus.Succeeded || payin.Status == TransactionStatus.Failed)
                    return Ok(payin.Status);

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
