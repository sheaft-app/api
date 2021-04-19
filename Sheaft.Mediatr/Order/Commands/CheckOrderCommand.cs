using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Order.Commands
{
    public class CheckOrderCommand : Command
    {
        protected CheckOrderCommand()
        {
            
        }
        [JsonConstructor]
        public CheckOrderCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class CheckOrderCommandHandler : CommandsHandler,
        IRequestHandler<CheckOrderCommand, Result>
    {
        private readonly RoutineOptions _routineOptions;

        public CheckOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result> Handle(CheckOrderCommand request, CancellationToken token)
        {
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            if (order.Status != OrderStatus.Created && order.Status != OrderStatus.Waiting)
                return Success();

            if (order.Status == OrderStatus.Waiting)
            {
                var pendingPayins = await _context.Payins
                    .Where(t => t.Order.Id == request.OrderId)
                    .ToListAsync(token);

                if (pendingPayins.Any(pt => pt.Status == TransactionStatus.Created || pt.Status == TransactionStatus.Waiting))
                    return Success();
            }
            
            if (order.CreatedOn.AddMinutes(_routineOptions.CheckOrderExpiredFromMinutes) < DateTimeOffset.UtcNow)
                return await _mediatr.Process(new ExpireOrderCommand(request.RequestUser) {OrderId = request.OrderId},
                    token);

            return Success();
        }
    }
}