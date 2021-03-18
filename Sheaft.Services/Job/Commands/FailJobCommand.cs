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

namespace Sheaft.Services.Job.Commands
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
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.FailJob(request.Reason);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}