﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class CompletePurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public CompletePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class CompletePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CompletePurchaseOrderCommand, Result>
    {
        public CompletePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CompletePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if(purchaseOrder.Vendor.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            purchaseOrder.Complete(request.SkipNotification);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}