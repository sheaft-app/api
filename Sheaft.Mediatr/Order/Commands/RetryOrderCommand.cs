using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.Payin.Commands;

namespace Sheaft.Mediatr.Order.Commands
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
        public RetryOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RetryOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(RetryOrderCommand request, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if(order.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if (order.Status != OrderStatus.Refused)
                return Failure<Guid>(MessageKind.Order_CannotRetry_NotIn_Refused_Status);

            var checkResult =
                await _mediatr.Process(
                    new CheckConsumerConfigurationCommand(request.RequestUser) {ConsumerId = order.User.Id}, token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult.Exception);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                order.SetStatus(OrderStatus.Waiting);
                await _context.SaveChangesAsync(token);

                var result = await _mediatr.Process(new CreateWebPayinCommand(request.RequestUser) {OrderId = order.Id},
                    token);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(token);
                    return Failure<Guid>(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success(result.Data);
            }
        }
    }
}