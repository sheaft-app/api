using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class LevelCommandsHandler : ResultsHandler,
        IRequestHandler<CreateLevelCommand, Result<Guid>>,
        IRequestHandler<UpdateLevelCommand, Result<bool>>,
        IRequestHandler<DeleteLevelCommand, Result<bool>>,
        IRequestHandler<RestoreLevelCommand, Result<bool>>
    {
        public LevelCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<LevelCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = new Level(Guid.NewGuid(), request.Name, request.RequiredPoints);
                
                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Level>(request.Id, token);
                entity.SetName(request.Name);
                entity.SetRequiredPoints(request.RequiredPoints);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Level>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RestoreLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Levels.SingleOrDefaultAsync(r => r.Id == request.Id, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}