﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class DeleteJobCommand : Command
    {
        protected DeleteJobCommand()
        {
            
        }
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
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            _context.Remove(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}