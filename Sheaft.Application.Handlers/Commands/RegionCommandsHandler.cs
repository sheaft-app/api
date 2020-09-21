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
using System.Linq;

namespace Sheaft.Application.Handlers
{
    public class RegionCommandsHandler : ResultsHandler,
        IRequestHandler<UpdateRegionCommand, Result<bool>>,
        IRequestHandler<UpdateRegionStatsCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;

        public RegionCommandsHandler(
            IAppDbContext context,
            ILogger<RegionCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<bool>> Handle(UpdateRegionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Regions.SingleOrDefaultAsync(c => c.Id == request.Id, token);

                entity.SetName(request.Name);
                entity.SetRequiredProducers(request.RequiredProducers);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UpdateRegionStatsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var region = await _context.Regions.SingleOrDefaultAsync(c => c.Id == request.Id, token);

                region.SetPoints(request.Points);
                region.SetPosition(request.Position);
                var consumersCount = await _context.Users.OfType<Consumer>().CountAsync(u => !u.RemovedOn.HasValue && u.Address.Department.Region.Id == request.Id, token);

                region.SetConsumersCount(consumersCount);

                region.SetProducersCount(request.Producers);
                region.SetStoresCount(request.Stores);

                _context.Update(region);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}