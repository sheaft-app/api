﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class RestorePurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public RestorePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class RestorePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<RestorePurchaseOrderCommand, Result>
    {
        public RestorePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RestorePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder =
                await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.PurchaseOrderId && a.RemovedOn.HasValue,
                    token);
            if(purchaseOrder.Vendor.Id != request.RequestUser.Id && purchaseOrder.Sender.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            _context.Restore(purchaseOrder);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}