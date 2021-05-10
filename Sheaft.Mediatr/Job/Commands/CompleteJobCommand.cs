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
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class CompleteJobCommand : Command
    {
        protected CompleteJobCommand()
        {
            
        }
        [JsonConstructor]
        public CompleteJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public string FileUrl { get; set; }
    }

    public class CompleteJobCommandHandler : CommandsHandler,
        IRequestHandler<CompleteJobCommand, Result>
    {
        public CompleteJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CompleteJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompleteJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.SetDownloadUrl(request.FileUrl);
            entity.CompleteJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}