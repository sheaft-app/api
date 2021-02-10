using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class FailOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public FailOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public Guid PayinId { get; set; }
    }
    public class FailOrderCommandHandler : CommandsHandler,
        IRequestHandler<FailOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public FailOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<FailOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(FailOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (order.Payin.Id != request.PayinId)
                    return Ok(true);

                order.SetStatus(OrderStatus.Refused);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new PayinFailedEvent(request.RequestUser) { PayinId = request.PayinId });
                return Ok(true);
            });
        }
    }
}
