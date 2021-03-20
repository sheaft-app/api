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

namespace Sheaft.Mediatr.Job.Commands
{
    public class UnarchiveJobCommand : Command
    {
        [JsonConstructor]
        public UnarchiveJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class UnarchiveJobCommandHandler : CommandsHandler,
        IRequestHandler<UnarchiveJobCommand, Result>
    {
        public UnarchiveJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UnarchiveJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UnarchiveJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.UnarchiveJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}