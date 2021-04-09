using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Order.Commands
{
    public class FailOrderCommand : Command
    {
        [JsonConstructor]
        public FailOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class FailOrderCommandHandler : CommandsHandler,
        IRequestHandler<FailOrderCommand, Result>
    {
        public FailOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<FailOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(FailOrderCommand request, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            order.SetStatus(OrderStatus.Refused);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}