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
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Job.Commands
{
    public class DeleteJobCommand : Command
    {
        [JsonConstructor]
        public DeleteJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }

    public class DeleteJobCommandHandler : CommandsHandler,
        IRequestHandler<DeleteJobCommand, Result>
    {
        public DeleteJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteJobCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            _context.Remove(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}