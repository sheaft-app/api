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

namespace Sheaft.Mediatr.User.Commands
{
    public class QueueExportUserDataCommand : Command<Guid>
    {
        protected QueueExportUserDataCommand()
        {
            
        }
        [JsonConstructor]
        public QueueExportUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = user.Id;
        }
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
            var sender = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(sender.Id != request.RequestUser.Id)
                return Failure<Guid>(MessageKind.Forbidden);

            var command = new ExportUserDataCommand(request.RequestUser) {JobId = Guid.NewGuid()};
            var entity = new Domain.Job(command.JobId, JobKind.ExportUserData, $"Export RGPD", sender, command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            _logger.LogInformation($"User RGPD data export successfully initiated by {request.UserId}");

            return Success(entity.Id);
        }
    }
}