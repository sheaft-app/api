using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Transactions.Commands
{
    public class QueueExportTransactionsCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
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
            var sender = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            if(sender.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            var entity = new Domain.Job(Guid.NewGuid(), JobKind.ExportUserTransactions, $"Export Virements", sender);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new ExportTransactionsCommand(request.RequestUser) {JobId = entity.Id, From = request.From, To = request.To});
            
            _logger.LogInformation($"User Transactions export successfully initiated by {request.UserId}");

            return Success(entity.Id);
        }
    }
}