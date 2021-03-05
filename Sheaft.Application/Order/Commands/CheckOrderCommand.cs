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
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Order.Commands
{
    public class CheckOrderCommand : Command
    {
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
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if (order.Status != OrderStatus.Created && order.Status != OrderStatus.Waiting)
                return Success();

            if (order.CreatedOn.AddMinutes(_routineOptions.CheckOrderExpiredFromMinutes) < DateTimeOffset.UtcNow)
                return await _mediatr.Process(new ExpireOrderCommand(request.RequestUser) {OrderId = request.OrderId},
                    token);

            return Success();
        }
    }
}