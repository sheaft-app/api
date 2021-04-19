using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.Mediatr.WebPayin.Commands
{
    public class CreateWebPayinForOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWebPayinForOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class PayOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateWebPayinForOrderCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly IOrderService _orderService;
        private readonly IIdentifierService _identifierService;
        private readonly IDeliveryService _deliveryService;

        public PayOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            IOrderService orderService,
            IIdentifierService identifierService,
            IDeliveryService deliveryService,
            ILogger<PayOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _orderService = orderService;
            _identifierService = identifierService;
            _deliveryService = deliveryService;
        }

        public async Task<Result<Guid>> Handle(CreateWebPayinForOrderCommand request, CancellationToken token)
        {
            var validationResult = await _orderService.ValidateConsumerOrderAsync(request.OrderId, request.RequestUser, token);
            if (!validationResult.Succeeded)
                return Failure<Guid>(validationResult);

            var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser), token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult);
            
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                if (!string.IsNullOrWhiteSpace(order.Reference))
                {
                    var referenceResult = await _identifierService.GetNextOrderReferenceAsync(token);
                    if (!referenceResult.Succeeded)
                        return Failure<Guid>(referenceResult.Exception);

                    order.SetReference(referenceResult.Data);
                }

                order.SetStatus(OrderStatus.Waiting);

                var wallet = await _context.Wallets.SingleOrDefaultAsync(c => c.User.Id == order.User.Id, token);
                if (order.TotalOnSalePrice < 1)
                    return Failure<Guid>(MessageKind.Order_Total_CannotBe_LowerThan, 1);

                var webPayin = new Domain.WebPayin(Guid.NewGuid(), wallet, order);

                await _context.AddAsync(webPayin, token);
                await _context.SaveChangesAsync(token);

                var legal = await _context.Legals.SingleOrDefaultAsync(c => c.Owner.Id == order.User.Id, token);
                var result = await _pspService.CreateWebPayinAsync(webPayin, legal.Owner, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

                webPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                webPayin.SetIdentifier(result.Data.Identifier);
                webPayin.SetRedirectUrl(result.Data.RedirectUrl);
                webPayin.SetStatus(result.Data.Status);
                webPayin.SetCreditedAmount(result.Data.Credited);
                webPayin.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(webPayin.Id);
            }
        }
    }
}