using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Job.Commands
{
    public class ResetJobCommand : Command
    {
        [JsonConstructor]
        public ResetJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
    public class ResetJobCommandHandler : CommandsHandler,
        IRequestHandler<ResetJobCommand, Result>
    {
        public ResetJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<ResetJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ResetJobCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.ResetJob();
            await _context.SaveChangesAsync(token);
            
            entity.EnqueueJobCommand(_mediatr, request.RequestUser);
            return Success();
        }
    }
}
