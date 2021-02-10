using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateWebPayinCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWebPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
    
    public class CreateWebPayinCommandHandler : CommandsHandler,
        IRequestHandler<CreateWebPayinCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CreateWebPayinCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreateWebPayinCommandHandler> logger)
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
                    return BadRequest<Guid>(MessageKind.Payin_CannotCreate_Order_Already_Validated);

                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.RequestUser.Id, token);

                if (order.TotalOnSalePrice < 1)
                    return BadRequest<Guid>(MessageKind.Order_Total_CannotBe_LowerThan, 1);

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
                webPayin.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                return Ok(webPayin.Id);
            });
        }
    }
}
