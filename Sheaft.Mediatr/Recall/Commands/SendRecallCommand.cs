using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Events.Recall;

namespace Sheaft.Mediatr.Recall.Commands
{
    public class SendRecallCommand : Command
    {
        protected SendRecallCommand()
        {
        }

        [JsonConstructor]
        public SendRecallCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RecallId { get; set; }
        public Guid JobId { get; set; }
    }

    public class SendRecallCommandHandler : CommandsHandler,
        IRequestHandler<SendRecallCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly RoleOptions _roleOptions;

        public SendRecallCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ICurrentUserService currentUserService,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<SendRecallCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _currentUserService = currentUserService;
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(SendRecallCommand request, CancellationToken token)
        {
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(job.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var recall = await _context.Recalls.SingleOrDefaultAsync(b => b.Id == request.RecallId, token);
            if (recall == null)
                return Failure("La campagne de rappel est introuvable.");
            
            try
            {
                job.StartJob();
                recall.StartSending();
                await _context.SaveChangesAsync(token);

                foreach (var client in recall.Clients.Where(c => !c.RecallSent))
                    _mediatr.Post(new RecallSentEvent(recall.Id, client.ClientId));
            
                recall.CompleteSending();
                job.CompleteJob();
                await _context.SaveChangesAsync(token);
                
                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message);
                recall.FailSending();
                await _context.SaveChangesAsync(token);
                
                return Failure("Une erreur est survenue pendant le traitement de l'envoi de la campagne de rappel.");
            }
        }
    }
}