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

namespace Sheaft.Mediatr.Observation.Commands
{
    public class DeleteObservationCommand : Command
    {
        protected DeleteObservationCommand()
        {
        }

        [JsonConstructor]
        public DeleteObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ObservationId { get; set; }
    }

    public class DeleteObservationCommandHandler : CommandsHandler,
        IRequestHandler<DeleteObservationCommand, Result>
    {
        public DeleteObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteObservationCommand request, CancellationToken token)
        {
            var observation = await _context.Observations.SingleOrDefaultAsync(b => b.Id == request.ObservationId, token);
            if (observation == null)
                return Failure("L'observation est introuvable.");

            _context.Remove(observation);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}