﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Order.Commands
{
    public class ExpireOrderCommand : Command
    {
        protected ExpireOrderCommand()
        {
            
        }
        [JsonConstructor]
        public ExpireOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class ExpireOrderCommandHandler : CommandsHandler,
        IRequestHandler<ExpireOrderCommand, Result>
    {
        public ExpireOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<ExpireOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ExpireOrderCommand request, CancellationToken token)
        {
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            order.SetStatus(OrderStatus.Expired);
            order.SetAsProcessed();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}