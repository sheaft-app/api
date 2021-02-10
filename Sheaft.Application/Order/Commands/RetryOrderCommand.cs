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
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RetryOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public RetryOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
    public class RetryOrderCommandHandler : CommandsHandler,
        IRequestHandler<RetryOrderCommand, Result<Guid>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public RetryOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<RetryOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(RetryOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (order.Status != OrderStatus.Refused)
                    return BadRequest<Guid>(MessageKind.Order_CannotRetry_NotIn_Refused_Status);

                var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser) { Id = request.RequestUser.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    order.SetStatus(OrderStatus.Waiting);
                    await _context.SaveChangesAsync(token);

                    var result = await _mediatr.Process(new CreateWebPayinCommand(request.RequestUser) { OrderId = order.Id }, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(result.Data);
                }
            });
        }
    }
}
