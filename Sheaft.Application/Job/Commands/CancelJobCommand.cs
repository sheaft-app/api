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
    public class CancelJobCommand : Command
    {
        [JsonConstructor]
        public CancelJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public string Reason { get; set; }
    }

    public class CancelJobCommandHandler : CommandsHandler,
        IRequestHandler<CancelJobCommand, Result>
    {
        public CancelJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.CancelJob(request.Reason);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}