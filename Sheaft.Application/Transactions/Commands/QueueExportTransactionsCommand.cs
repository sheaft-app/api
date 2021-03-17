using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Transactions.Commands
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