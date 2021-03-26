using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Region.Commands
{
    public class UpdateRegionStatsCommand : Command
    {
        [JsonConstructor]
        public UpdateRegionStatsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RegionId { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }

    public class UpdateRegionStatsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateRegionStatsCommand, Result>
    {
        public UpdateRegionStatsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateRegionStatsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateRegionStatsCommand request, CancellationToken token)
        {
            var region = await _context.Regions.SingleOrDefaultAsync(c => c.Id == request.RegionId, token);

            region.SetPoints(request.Points);
            region.SetPosition(request.Position);
            var consumersCount = await _context.Users.OfType<Domain.Consumer>()
                .CountAsync(u => !u.RemovedOn.HasValue && u.Address.Department.Region.Id == request.RegionId, token);

            region.SetConsumersCount(consumersCount);

            region.SetProducersCount(request.Producers);
            region.SetStoresCount(request.Stores);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}