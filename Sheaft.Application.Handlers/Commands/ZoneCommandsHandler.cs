using Sheaft.Application.Commands;
using Sheaft.Infrastructure.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class ZoneCommandsHandler : CommandsHandler,
        IRequestHandler<UpdateZoneProgressCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;

        public ZoneCommandsHandler(
            IAppDbContext context,
            ILogger<ZoneCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<CommandResult<bool>> Handle(UpdateZoneProgressCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var pointsPerRegions = await _context.RegionPoints.ToListAsync(token);
                var pointsPerDepartments = await _context.DepartmentPoints.ToListAsync(token);

                var producersPerDepartments = await _context.DepartmentProducers.ToListAsync(token);
                var storesPerDepartments = await _context.DepartmentStores.ToListAsync(token);

                foreach (var pointsPerRegion in pointsPerRegions)
                {
                    var region = await _context.GetByIdAsync<Region>(pointsPerRegion.RegionId, token);

                    region.SetPoints(pointsPerRegion.Points ?? 0);
                    region.SetPosition((int)pointsPerRegion.Position);
                    region.SetConsumersCount(pointsPerRegion.Users);

                    var producersCount = 0;
                    var storesCount = 0;

                    foreach (var pointsPerDepartment in pointsPerDepartments.Where(d => d.RegionId == region.Id))
                    {
                        var department = await _context.GetByIdAsync<Department>(pointsPerDepartment.DepartmentId, token);
                        var level = (await _context.GetAsync<Level>(c => c.RequiredPoints > pointsPerDepartment.Points, token)).OrderBy(c => c.RequiredPoints).FirstOrDefault();

                        department.SetLevel(level);
                        department.SetPoints(pointsPerDepartment.Points ?? 0);
                        department.SetPosition((int)pointsPerDepartment.Position);
                        department.SetConsumersCount(pointsPerDepartment.Users);

                        var producerCount = producersPerDepartments.FirstOrDefault(p => p.DepartmentCode == department.Code);
                        if (producerCount != null)
                        {
                            producersCount += (producerCount.Created ?? 0);
                            department.SetProducersCount(producerCount.Created ?? 0);
                        }

                        var storeCount = storesPerDepartments.FirstOrDefault(p => p.DepartmentCode == department.Code);
                        if (storeCount != null)
                        {
                            storesCount += (storeCount.Created ?? 0);
                            department.SetStoresCount(storeCount.Created ?? 0);
                        }

                        _context.Update(department);
                    }

                    region.SetProducersCount(producersCount);
                    region.SetStoresCount(storesCount);

                    _context.Update(region);
                    await _context.SaveChangesAsync(token);
                }
                
                return OkResult(true);
            });
        }
    }
}