using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Recall.Commands
{
    public class QueueSendRecallCommand : Command<Guid>
    {
        protected QueueSendRecallCommand()
        {
        }

        [JsonConstructor]
        public QueueSendRecallCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RecallId { get; set; }
    }

    public class QueueSendRecallCommandHandler : CommandsHandler,
        IRequestHandler<QueueSendRecallCommand, Result<Guid>>
    {
        public QueueSendRecallCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueSendRecallCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueSendRecallCommand request, CancellationToken token)
        {
            var recall = await _context.Recalls.SingleOrDefaultAsync(b => b.Id == request.RecallId, token);
            if (recall == null)
                return Failure<Guid>("La campagne de rappel est introuvable.");
           
            if(recall.ProducerId != request.RequestUser.Id)
                return Failure<Guid>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var command = new SendRecallCommand(request.RequestUser) {JobId = Guid.NewGuid(), RecallId = recall.Id};
            var entity = new Domain.Job(command.JobId, JobKind.SendRecalls, $"Envoi campagne de rappel", recall.Producer, command);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(command);
            return Success(entity.Id);
        }
    }
}