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
    public class FailJobsCommand : Command
    {
        [JsonConstructor]
        public FailJobsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> JobIds { get; set; }
        public string Reason { get; set; }
    }

    public class FailJobsCommandHandler : CommandsHandler,
        IRequestHandler<FailJobsCommand, Result>
    {
        public FailJobsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<FailJobsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(FailJobsCommand request,
            CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var jobId in request.JobIds)
                {
                    var result =
                        await _mediatr.Process(
                            new FailJobCommand(request.RequestUser) {JobId = jobId, Reason = request.Reason}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}