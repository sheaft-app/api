﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Order.Commands
{
    public class FailOrderCommand : Command
    {
        protected FailOrderCommand()
        {
            
        }
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
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            if (order.Processed)
                return Success();
            
            order.SetStatus(OrderStatus.Refused);
            order.SetAsProcessed();
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}