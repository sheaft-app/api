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
    public class ArchiveJobCommand : Command
    {
        [JsonConstructor]
        public ArchiveJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class ArchiveJobCommandHandler : CommandsHandler,
        IRequestHandler<ArchiveJobCommand, Result>
    {
        public ArchiveJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ArchiveJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ArchiveJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.ArchiveJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}