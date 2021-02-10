﻿using System;
using System.Collections.Generic;
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
    public class DeletePurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public DeletePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }

    public class DeletePurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<DeletePurchaseOrdersCommand, Result>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public DeletePurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<DeletePurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result> Handle(DeletePurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var purchaseOrderId in request.Ids)
                {
                    var result =
                        await _mediatr.Process(
                            new DeletePurchaseOrderCommand(request.RequestUser) {Id = purchaseOrderId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}