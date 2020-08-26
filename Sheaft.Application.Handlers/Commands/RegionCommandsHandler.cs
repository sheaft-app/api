using Sheaft.Infrastructure.Interop;
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
    public class RegionCommandsHandler : CommandsHandler,
        IRequestHandler<UpdateRegionCommand, CommandResult<bool>>,
        IRequestHandler<UpdateRegionStatsCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;

        public RegionCommandsHandler(
            IAppDbContext context,
            ILogger<RegionCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<CommandResult<bool>> Handle(UpdateRegionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Region>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetRequiredProducers(request.RequiredProducers);

                _context.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateRegionStatsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var region = await _context.GetByIdAsync<Region>(request.Id, token);

                region.SetPoints(request.Points);
                region.SetPosition(request.Position);
                var users = await _context.Users.CountAsync(u => !u.RemovedOn.HasValue && u.UserType == Interop.Enums.UserKind.Consumer && u.Department != null && u.Department.Region != null && u.Department.Region.Id == request.Id, token);

                region.SetConsumersCount(users);

                region.SetProducersCount(request.Producers);
                region.SetStoresCount(request.Stores);

                _context.Update(region);
                await _context.SaveChangesAsync(token);

                return OkResult(true);
            });
        }
    }
}