using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Transactions.Commands
{
    public class QueueExportTransactionsCommand : Command<Guid>
    {
        protected QueueExportTransactionsCommand()
        {
            
        }
        [JsonConstructor]
        public QueueExportTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }

        public Guid UserId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class QueueExportTransactionsCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportTransactionsCommand, Result<Guid>>
    {
        public QueueExportTransactionsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueExportTransactionsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueExportTransactionsCommand request, CancellationToken token)
        {
            var sender = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(sender.Id != request.RequestUser.Id)
                return Failure<Guid>(MessageKind.Forbidden);

            var command = new ExportTransactionsCommand(request.RequestUser)
                {JobId = Guid.NewGuid(), From = request.From, To = request.To};
            var entity = new Domain.Job(command.JobId, JobKind.ExportUserTransactions, $"Export Virements", sender, command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            
            _logger.LogInformation($"User Transactions export successfully initiated by {request.UserId}");

            return Success(entity.Id);
        }
    }
}