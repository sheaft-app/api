using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Job.Commands
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