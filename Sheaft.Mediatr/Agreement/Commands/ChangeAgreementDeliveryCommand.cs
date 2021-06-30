using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class ChangeAgreementDeliveryCommand : Command
    {
        protected ChangeAgreementDeliveryCommand()
        {
        }

        [JsonConstructor]
        public ChangeAgreementDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public Guid DeliveryId { get; set; }
    }

    public class ChangeAgreementDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<ChangeAgreementDeliveryCommand, Result>
    {
        private readonly RoleOptions _roleOptions;
        
        public ChangeAgreementDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ChangeAgreementDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(ChangeAgreementDeliveryCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(e => e.Id == request.AgreementId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas authorisé à accéder à cette ressource.");
            
            var deliveryMode = await _context.DeliveryModes.SingleAsync(c => c.Id == request.DeliveryId, token);
            entity.ChangeDelivery(deliveryMode);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}