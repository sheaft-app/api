using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Producer.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Transfer.Commands
{
    public class CreateTransferCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class CreateTransferCommandHandler : CommandsHandler,
        IRequestHandler<CreateTransferCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CreateTransferCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreateTransferCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateTransferCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if (purchaseOrder.Status != PurchaseOrderStatus.Delivered)
                return Failure<Guid>(MessageKind.Transfer_CannotCreate_PurchaseOrder_Invalid_Status);

            if (purchaseOrder.Transfer != null && purchaseOrder.Transfer.Status == TransactionStatus.Succeeded)
                return Failure<Guid>(MessageKind.Transfer_CannotCreate_AlreadyProcessed);

            if (purchaseOrder.Transfer != null && purchaseOrder.Transfer.Status != TransactionStatus.Failed)
                return Failure<Guid>(MessageKind.Transfer_CannotCreate_Pending_Transfer);

            var checkResult =
                await _mediatr.Process(
                    new CheckProducerConfigurationCommand(request.RequestUser) {ProducerId = purchaseOrder.Vendor.Id},
                    token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult.Exception);

            var debitedWallet = await _context.GetSingleAsync<Domain.Wallet>(c => c.User.Id == purchaseOrder.Sender.Id, token);
            var creditedWallet =
                await _context.GetSingleAsync<Domain.Wallet>(c => c.User.Id == purchaseOrder.Vendor.Id, token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var transfer = new Domain.Transfer(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);
                await _context.AddAsync(transfer, token);
                await _context.SaveChangesAsync(token);

                purchaseOrder.SetTransfer(transfer);

                var result = await _pspService.CreateTransferAsync(transfer, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

                transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                transfer.SetIdentifier(result.Data.Identifier);
                transfer.SetStatus(result.Data.Status);
                transfer.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(transfer.Id);
            }
        }
    }
}