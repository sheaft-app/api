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

namespace Sheaft.Mediatr.PurchaseOrderDelivery.Commands
{
    public class StartPurchaseOrderDeliveryCommand : Command
    {
        protected StartPurchaseOrderDeliveryCommand()
        {
        }

        [JsonConstructor]
        public StartPurchaseOrderDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderDeliveryId { get; set; }
    }

    public class StartPurchaseOrderDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<StartPurchaseOrderDeliveryCommand, Result>
    {
        public StartPurchaseOrderDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartPurchaseOrderDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartPurchaseOrderDeliveryCommand request, CancellationToken token)
        {
            var purchaseOrderDelivery = await _context.Set<Domain.PurchaseOrderDelivery>()
                .SingleOrDefaultAsync(c => c.Id == request.PurchaseOrderDeliveryId, token);
            if (purchaseOrderDelivery == null)
                return Failure(MessageKind.NotFound);

            purchaseOrderDelivery.StartDelivery();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}