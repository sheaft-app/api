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
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.DeliveryClosing.Commands
{
    public class CreateDeliveryClosingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDeliveryClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public ClosingInput Closing { get; set; }
    }

    public class CreateDeliveryClosingCommandHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryClosingCommand, Result<Guid>>
    {
        public CreateDeliveryClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateDeliveryClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateDeliveryClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.DeliveryMode>(request.DeliveryId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.AddClosing(request.Closing.From, request.Closing.To, request.Closing.Reason);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}