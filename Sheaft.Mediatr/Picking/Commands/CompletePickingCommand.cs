﻿using System;
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
using Sheaft.Core.Exceptions;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Picking.Commands
{
    public class CompletePickingCommand : Command
    {
        protected CompletePickingCommand()
        {
        }

        [JsonConstructor]
        public CompletePickingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PickingId { get; set; }
    }

    public class CompletePickingCommandHandler : CommandsHandler,
        IRequestHandler<CompletePickingCommand, Result>
    {
        public CompletePickingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CompletePickingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompletePickingCommand request, CancellationToken token)
        {
            var picking = await _context.Pickings
                .SingleOrDefaultAsync(c => c.Id == request.PickingId, token);
            if (picking == null)
                return Failure("La préparation est introuvable.");
            
            if (picking.ProductsToPrepare.Select(pp => pp.ProductId).Except(picking.PreparedProducts.Select(pp => pp.ProductId)).Any())
                return Failure("Certain produits n'ont pas été préparés.");
            
            if (picking.PreparedProducts.Any(p => !p.PreparedOn.HasValue))
                return Failure("Certaines quantités de produits n'ont pas été validées.");
            
            picking.Complete();
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}