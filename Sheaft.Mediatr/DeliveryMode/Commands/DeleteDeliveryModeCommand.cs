﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class DeleteDeliveryModeCommand : Command
    {
        protected DeleteDeliveryModeCommand()
        {
        }

        [JsonConstructor]
        public DeleteDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryModeId { get; set; }
    }

    public class DeleteDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryModeCommand, Result>
    {
        public DeleteDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryModeCommand request, CancellationToken token)
        {
            var entity = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryModeId, token);
            if (entity.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var agreements =
                await _context.Agreements.Where(a => a.DeliveryModeId == entity.Id).ToListAsync(token);
            if (agreements.Any(a => a.Status == AgreementStatus.Accepted))
                return Failure(
                    $"Impossible de supprimer ce mode de livraison, {agreements.Count(a => a.Status == AgreementStatus.Accepted)} partenariats y sont rattaché");

            var orderDeliveries = await _context.Set<OrderDelivery>()
                .Where(o => o.DeliveryModeId == entity.Id)
                .ToListAsync(token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var agreement in agreements)
                {
                    result =
                        await _mediatr.Process(
                            new DeleteAgreementCommand(request.RequestUser) {AgreementId = agreement.Id}, token);

                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                foreach (var orderDelivery in orderDeliveries.ToList())
                    _context.Remove(orderDelivery);

                _context.Remove(entity);

                var canDirectSell = await _context.DeliveryModes.AnyAsync(
                    c => !c.RemovedOn.HasValue && c.ProducerId == entity.ProducerId &&
                         (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm ||
                          c.Kind == DeliveryKind.Market), token);

                entity.Producer.SetCanDirectSell(canDirectSell);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}