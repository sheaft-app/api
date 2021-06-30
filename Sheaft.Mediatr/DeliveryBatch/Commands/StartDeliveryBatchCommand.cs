using System;
using System.Collections.Generic;
using System.Linq;
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
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Core.Enums;
using Sheaft.Domain.Events.DeliveryBatch;
using Sheaft.Mediatr.Delivery.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class StartDeliveryBatchCommand : Command
    {
        protected StartDeliveryBatchCommand()
        {
        }

        [JsonConstructor]
        public StartDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool StartFirstDelivery { get; set; }
    }

    public class StartDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<StartDeliveryBatchCommand, Result>
    {
        public StartDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            deliveryBatch.StartBatch();
            if (request.StartFirstDelivery)
            {
                var delivery = deliveryBatch.Deliveries.OrderBy(d => d.Position).First();
                delivery.StartDelivery();
            }
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}