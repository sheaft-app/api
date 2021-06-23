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

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class SkipDeliveryCommand : Command
    {
        protected SkipDeliveryCommand()
        {
        }

        [JsonConstructor]
        public SkipDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class SkipDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<SkipDeliveryCommand, Result>
    {
        public SkipDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SkipDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SkipDeliveryCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);
            if (delivery == null)
                return Failure(MessageKind.NotFound);

            delivery.SkipDelivery();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}