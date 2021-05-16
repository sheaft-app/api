using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class AcceptAgreementCommand : Command
    {
        protected AcceptAgreementCommand()
        {
        }

        [JsonConstructor]
        public AcceptAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public Guid? CatalogId { get; set; }
        public Guid? DeliveryId { get; set; }
    }

    public class AcceptAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<AcceptAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public AcceptAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<AcceptAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(AcceptAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleAsync(e => e.Id == request.AgreementId, token);
            if (request.RequestUser.IsInRole(_roleOptions.Producer.Value) &&
                entity.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            if (request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.StoreId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            var alreadyAcceptedAgreement =
                await _context.Agreements.SingleOrDefaultAsync(
                    a => a.Id != request.AgreementId && a.ProducerId == entity.ProducerId &&
                         a.StoreId == entity.StoreId && a.Status == AgreementStatus.Accepted, token);
            if (alreadyAcceptedAgreement != null)
                return Failure(MessageKind.AlreadyExists);

            Domain.DeliveryMode delivery = null;
            if (request.DeliveryId.HasValue)
                delivery = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryId, token);

            var currentUser = await _context.Users.SingleAsync(c => c.Id == request.RequestUser.Id, token);
            entity.AcceptAgreement(delivery, currentUser.Kind);

            if (!entity.CatalogId.HasValue && request.CatalogId.HasValue)
            {
                var catalog = await _context.Catalogs.SingleAsync(e => e.Id == request.CatalogId.Value, token);
                entity.AssignCatalog(catalog);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}