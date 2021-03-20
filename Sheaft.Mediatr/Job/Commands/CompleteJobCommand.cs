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
    public class CompleteJobCommand : Command
    {
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
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.SetDownloadUrl(request.FileUrl);
            entity.CompleteJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}