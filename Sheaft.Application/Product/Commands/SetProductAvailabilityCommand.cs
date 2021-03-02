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

namespace Sheaft.Application.Product.Commands
{
    public class SetProductAvailabilityCommand : Command
    {
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
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            entity.SetAvailable(request.Available);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}