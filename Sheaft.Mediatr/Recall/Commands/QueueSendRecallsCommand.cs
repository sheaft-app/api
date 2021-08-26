using System;
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

namespace Sheaft.Mediatr.Recall.Commands
{
    public class QueueSendRecallsCommand : Command<IEnumerable<Guid>>
    {
        protected QueueSendRecallsCommand()
        {
        }

        [JsonConstructor]
        public QueueSendRecallsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> RecallIds { get; set; }
    }

    public class QueueSendRecallsCommandHandler : CommandsHandler,
        IRequestHandler<QueueSendRecallsCommand, Result<IEnumerable<Guid>>>
    {
        public QueueSendRecallsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueSendRecallsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(QueueSendRecallsCommand request, CancellationToken token)
        {
            var recalls = await _context.Recalls.Where(b => request.RecallIds.Contains(b.Id)).ToListAsync(token);
            if (recalls == null || !recalls.Any())
                return Failure<IEnumerable<Guid>>("La campagne de rappel est introuvable.");

            var jobIds = new List<Guid>();
            string error = null;
            foreach (var recall in recalls)
            {
                if (recall.Status != RecallStatus.Waiting)
                {
                    error = "La campagne de rappel n'est pas en attente.";
                    break;
                }

                if (recall.ProducerId != request.RequestUser.Id)
                {
                    error = "Vous n'êtes pas autorisé à accéder à cette ressource.";
                    break;
                }

                var command = new SendRecallCommand(request.RequestUser) {JobId = Guid.NewGuid(), RecallId = recall.Id};
                var entity = new Domain.Job(command.JobId, JobKind.SendRecalls, $"Envoi campagne de rappel",
                    recall.Producer, command);
                
                recall.SetAsReady();

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(command);
                jobIds.Add(entity.Id);
            }

            if (!string.IsNullOrEmpty(error))
                return Failure<IEnumerable<Guid>>(error);

            return Success<IEnumerable<Guid>>(jobIds);
        }
    }
}