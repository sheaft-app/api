using System;
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

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class QueueExportPurchaseOrdersCommand : Command<Guid>
    {
        protected QueueExportPurchaseOrdersCommand()
        {
            
        }
        [JsonConstructor]
        public QueueExportPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Name { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class QueueExportPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportPurchaseOrdersCommand, Result<Guid>>
    {
        public QueueExportPurchaseOrdersCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueExportPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueExportPurchaseOrdersCommand request, CancellationToken token)
        {
            var sender = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(sender.Id != request.RequestUser.Id)
                return Failure<Guid>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var command = new ExportPurchaseOrdersCommand(request.RequestUser)
                {JobId = Guid.NewGuid(), From = request.From, To = request.To};
            
            var entity = new Domain.Job(command.JobId, JobKind.ExportUserPurchaseOrders, request.Name ?? $"Export du {DateTimeOffset.UtcNow:dd/MM/yyyy} à {DateTimeOffset.UtcNow:HH:mm}", sender, command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            
            return Success(entity.Id);
        }
    }
}