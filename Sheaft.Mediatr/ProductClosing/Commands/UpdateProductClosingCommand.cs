﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.ProductClosing.Commands
{
    public class UpdateProductClosingCommand : Command
    {
        [JsonConstructor]
        public UpdateProductClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ClosingId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateProductClosingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductClosingCommand, Result>
    {
        public UpdateProductClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateProductClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateProductClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetSingleAsync<Domain.Product>(c => c.Closings.Any(cc => cc.Id == request.ClosingId), token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            var closing = entity.Closings.SingleOrDefault(c => c.Id == request.ClosingId);
            if(closing == null)
                throw SheaftException.NotFound();
            
            closing.ChangeClosedDates(request.From, request.To);
            closing.SetReason(request.Reason);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}