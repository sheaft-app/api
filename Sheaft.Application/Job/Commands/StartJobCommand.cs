﻿using System;
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
    public class StartJobCommand : Command
    {
        [JsonConstructor]
        public StartJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class StartJobCommandHandler : CommandsHandler,
        IRequestHandler<StartJobCommand, Result>
    {
        public StartJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartJobCommand request,
            CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.Id, token);
            entity.StartJob();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}