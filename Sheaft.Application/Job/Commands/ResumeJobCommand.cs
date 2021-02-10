using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Job.Commands
{
    public class ResumeJobCommand : Command
    {
        [JsonConstructor]
        public ResumeJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class ResumeJobCommandHandler : CommandsHandler,
        IRequestHandler<ResumeJobCommand, Result>
    {
        public ResumeJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ResumeJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ResumeJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            entity.ResumeJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}