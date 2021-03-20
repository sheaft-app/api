using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.User.Commands
{
    public class QueueExportUserDataCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class QueueExportUserDataCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportUserDataCommand, Result<Guid>>
    {
        public QueueExportUserDataCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueExportUserDataCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueExportUserDataCommand request, CancellationToken token)
        {
            var sender = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            if(sender.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            var entity = new Domain.Job(Guid.NewGuid(), JobKind.ExportUserData, $"Export RGPD", sender);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new ExportUserDataCommand(request.RequestUser) {JobId = entity.Id});
            ;
            _logger.LogInformation($"User RGPD data export successfully initiated by {request.UserId}");

            return Success(entity.Id);
        }
    }
}