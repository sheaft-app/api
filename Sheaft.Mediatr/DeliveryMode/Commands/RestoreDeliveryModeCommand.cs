using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class RestoreDeliveryModeCommand : Command
    {
        protected RestoreDeliveryModeCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryModeId { get; set; }
    }

    public class RestoreDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<RestoreDeliveryModeCommand, Result>
    {
        public RestoreDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreDeliveryModeCommand request, CancellationToken token)
        {
            var entity =
                await _context.DeliveryModes.SingleOrDefaultAsync(a => a.Id == request.DeliveryModeId && a.RemovedOn.HasValue,
                    token);
            if(entity.ProducerId != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            _context.Restore(entity);    
            
            if (entity.Kind is DeliveryKind.Collective or DeliveryKind.Farm or DeliveryKind.Market)
                entity.Producer.SetCanDirectSell(true);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}