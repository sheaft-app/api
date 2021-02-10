﻿using System;
using System.Collections.Generic;
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
    public class StartJobsCommand : Command
    {
        [JsonConstructor]
        public StartJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }

    public class StartJobsCommandHandler : CommandsHandler,
        IRequestHandler<StartJobsCommand, Result>
    {
        public StartJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var jobId in request.Ids)
                {
                    var result = await _mediatr.Process(new StartJobCommand(request.RequestUser) {Id = jobId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}