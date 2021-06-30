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
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class DeleteDeliveryBatchCommand : Command
    {
        protected DeleteDeliveryBatchCommand()
        {
        }
        
        [JsonConstructor]
        public DeleteDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class DeleteDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryBatchCommand, Result>
    {
        public DeleteDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            _context.Remove(deliveryBatch);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}