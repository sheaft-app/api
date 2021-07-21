using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Transaction.Commands;

namespace Sheaft.Mediatr.Billing.Commands
{
    public class QueueExportTimeRangedBillingsCommand : Command<Guid>
    {
        protected QueueExportTimeRangedBillingsCommand()
        {
        }

        [JsonConstructor]
        public QueueExportTimeRangedBillingsCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
        public string Name { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class QueueExportAccountingTimeRangeCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportTimeRangedBillingsCommand, Result<Guid>>
    {
        public QueueExportAccountingTimeRangeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueExportAccountingTimeRangeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueExportTimeRangedBillingsCommand request, CancellationToken token)
        {
            var sender = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if (sender.Id != request.RequestUser.Id)
                return Failure<Guid>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var command = new ExportTimeRangedBillingsCommand(request.RequestUser)
                {JobId = Guid.NewGuid(), From = request.From, To = request.To, Kinds = request.Kinds};

            var entity = new Domain.Job(command.JobId, JobKind.ExportUserBillingsTimeRange,
                request.Name ?? $"Export comptabilité du {request.From:dd/MM/yyyy} à {request.To:HH:mm}", sender,
                command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            return Success(entity.Id);
        }
    }
}