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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class ArchiveJobCommand : Command
    {
        protected ArchiveJobCommand()
        {
            
        }
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
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if (entity.UserId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            entity.ArchiveJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}