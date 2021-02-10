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
    public class UpdateJobCommand : Command
    {
        [JsonConstructor]
        public UpdateJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateJobCommandHandler : CommandsHandler,
        IRequestHandler<UpdateJobCommand, Result>
    {
        public UpdateJobCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateJobCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Job>(request.Id, token);
            entity.SetName(request.Name);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}