using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CreatePayinCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class CreatePayinCommandHandler : CommandsHandler,
        IRequestHandler<CreatePayinCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreatePayinCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<CreatePayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePayinCommand request, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if (order.Status == OrderStatus.Validated)
                return Failure<Guid>(MessageKind.Payin_CannotCreate_Order_Already_Validated);

            var wallet = await _context.GetSingleAsync<Domain.Wallet>(c => c.User.Id == order.User.Id, token);

            if (order.TotalOnSalePrice < 1)
                return Failure<Guid>(MessageKind.Order_Total_CannotBe_LowerThan, 1);

            var webPayin = new WebPayin(Guid.NewGuid(), wallet, order);

            await _context.AddAsync(webPayin, token);
            await _context.SaveChangesAsync(token);

            order.SetPayin(webPayin);

            var legal = await _context.GetSingleAsync<Domain.Legal>(c => c.Owner.Id == order.User.Id, token);
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
            return Success(webPayin.Id);
        }
    }
}