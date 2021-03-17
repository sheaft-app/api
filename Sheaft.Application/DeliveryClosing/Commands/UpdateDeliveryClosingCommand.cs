using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.DeliveryClosing.Commands
{
    public class UpdateDeliveryClosingCommand : Command
    {
        [JsonConstructor]
        public UpdateDeliveryClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ClosingId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateDeliveryClosingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDeliveryClosingCommand, Result>
    {
        public UpdateDeliveryClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDeliveryClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateDeliveryClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetSingleAsync<Domain.DeliveryMode>(c => c.Closings.Any(cc => cc.Id == request.ClosingId), token);
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