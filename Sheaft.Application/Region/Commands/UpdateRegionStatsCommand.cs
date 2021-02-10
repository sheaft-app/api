using Newtonsoft.Json;
using Sheaft.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class UpdateRegionStatsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateRegionStatsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
    
    public class UpdateRegionStatsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateRegionStatsCommand, Result<bool>>
    {
        public UpdateRegionStatsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateRegionStatsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateRegionStatsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var region = await _context.Regions.SingleOrDefaultAsync(c => c.Id == request.Id, token);

                region.SetPoints(request.Points);
                region.SetPosition(request.Position);
                var consumersCount = await _context.Users.OfType<Consumer>().CountAsync(u => !u.RemovedOn.HasValue && u.Address.Department.Region.Id == request.Id, token);

                region.SetConsumersCount(consumersCount);

                region.SetProducersCount(request.Producers);
                region.SetStoresCount(request.Stores);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
