﻿using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.Billing.Commands
{
    public class QueueExportBillingsCommand : Command<Guid>
    {
        protected QueueExportBillingsCommand()
        {
        }

        [JsonConstructor]
        public QueueExportBillingsCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public IEnumerable<Guid> DeliveryIds { get; set; }
        public string Name { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class QueueExportAccountingDeliveriesCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportBillingsCommand, Result<Guid>>
    {
        public QueueExportAccountingDeliveriesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueExportAccountingDeliveriesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueExportBillingsCommand request, CancellationToken token)
        {
            var sender = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if (sender.Id != request.RequestUser.Id)
                return Failure<Guid>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            if (request.DeliveryIds == null || !request.DeliveryIds.Any())
                return Failure<Guid>("Vous devez selectionner les livraisons à exporter.");

            var command = new ExportBillingsCommand(request.RequestUser)
                {JobId = Guid.NewGuid(), DeliveryIds = request.DeliveryIds};

            var entity = new Domain.Job(command.JobId, JobKind.ExportUserBillings,
                request.Name ?? $"Export comptabilité spécifique", sender, command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            return Success(entity.Id);
        }
    }
}