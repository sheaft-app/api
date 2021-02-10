using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class ExpireOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public ExpireOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
    public class ExpireOrderCommandHandler : CommandsHandler,
        IRequestHandler<ExpireOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public ExpireOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<ExpireOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }
        public async Task<Result<bool>> Handle(ExpireOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                order.SetStatus(OrderStatus.Expired);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
