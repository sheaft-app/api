﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class ResumeJobCommand : Command
    {
        protected ResumeJobCommand()
        {
            
        }
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
            var entity = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if(entity.UserId != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.ResumeJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}