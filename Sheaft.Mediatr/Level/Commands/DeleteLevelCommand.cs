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

namespace Sheaft.Mediatr.Level.Commands
{
    public class DeleteLevelCommand : Command
    {
        protected DeleteLevelCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LevelId { get; set; }
    }

    public class DeleteLevelCommandHandler : CommandsHandler,
        IRequestHandler<DeleteLevelCommand, Result>
    {
        public DeleteLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteLevelCommand request, CancellationToken token)
        {
            var entity = await _context.Levels.SingleAsync(e => e.Id == request.LevelId, token);
            _context.Remove(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}