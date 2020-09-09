﻿using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace Sheaft.Application.Handlers
{

    public class LevelCommandsHandler : CommandsHandler,
        IRequestHandler<CreateLevelCommand, Result<Guid>>,
        IRequestHandler<UpdateLevelCommand, Result<bool>>,
        IRequestHandler<DeleteLevelCommand, Result<bool>>,
        IRequestHandler<RestoreLevelCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;

        public LevelCommandsHandler(
            IAppDbContext context,
            ILogger<LevelCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = new Level(Guid.NewGuid(), request.Name, request.RequiredPoints);
                
                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Level>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetRequiredPoints(request.RequiredPoints);

                _context.Update(entity);
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeleteLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Level>(request.Id, token);
                entity.Remove();

                _context.Remove(entity);
                var results = await _context.SaveChangesAsync(token);

                return Ok(results > 0);
            });
        }

        public async Task<Result<bool>> Handle(RestoreLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Levels.SingleOrDefaultAsync(r => r.Id == request.Id, token);
                entity.Restore();

                _context.Update(entity);
                var results = await _context.SaveChangesAsync(token);

                return Ok(results > 0);
            });
        }
    }
}