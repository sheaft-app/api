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
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Product.Commands
{
    public class SetProductAvailabilityCommand : Command
    {
        protected SetProductAvailabilityCommand()
        {
            
        }
        [JsonConstructor]
        public SetProductAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public bool Available { get; set; }
    }

    public class SetProductAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetProductAvailabilityCommand, Result>
    {
        public SetProductAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetProductAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetProductAvailabilityCommand request, CancellationToken token)
        {
            var entity = await _context.Products.SingleAsync(e => e.Id == request.ProductId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            entity.SetAvailable(request.Available);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}