using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckOrderCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
        public Guid OrderId { get; set; }
    }
    public class CheckOrderCommandHandler : CommandsHandler,
        IRequestHandler<CheckOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public CheckOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payinRefund = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (payinRefund.Status != OrderStatus.Created && payinRefund.Status != OrderStatus.Waiting)
                    return Ok(false);

                if (payinRefund.CreatedOn.AddMinutes(_routineOptions.CheckOrderExpiredFromMinutes) < DateTimeOffset.UtcNow)
                    return await _mediatr.Process(new ExpireOrderCommand(request.RequestUser) { OrderId = request.OrderId }, token);

                return Ok(true);
            });
        }
    }
}
