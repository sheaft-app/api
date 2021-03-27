﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Job.Commands
{
    public class UpdateJobCommand : Command
    {
        [JsonConstructor]
        public UpdateJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
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
            var entity = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if(entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.SetName(request.Name);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}