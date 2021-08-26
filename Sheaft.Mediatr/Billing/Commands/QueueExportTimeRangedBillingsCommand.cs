using System;
using System.Collections.Generic;
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
                request.Name ?? $"Export comptabilité du {request.From:dd/MM/yyyy} au {request.To:dd/MM/yyyy}", sender,
                command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            return Success(entity.Id);
        }
    }
}