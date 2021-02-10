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
    public class FailJobCommand : Command
    {
        [JsonConstructor]
        public FailJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public string Reason { get; set; }
    }

    public class FailJobCommandHandler : CommandsHandler,
        IRequestHandler<FailJobCommand, Result>
    {
        public FailJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<FailJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(FailJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            entity.FailJob(request.Reason);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}