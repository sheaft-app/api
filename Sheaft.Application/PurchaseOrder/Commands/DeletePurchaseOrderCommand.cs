﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class DeletePurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public DeletePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class DeletePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeletePurchaseOrderCommand, Result>
    {
        public DeletePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<DeletePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeletePurchaseOrderCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}