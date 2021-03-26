using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryClosing.Commands
{
    public class CreateDeliveryClosingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDeliveryClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public CreateClosingDto Closing { get; set; }
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