using System.Linq;
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
using Sheaft.Domain.Views;
using Sheaft.Mediatr.Department.Commands;
using Sheaft.Mediatr.Region.Commands;

namespace Sheaft.Mediatr.Zone.Commands
{
    public class UpdateZonesProgressCommand : Command
    {
        protected UpdateZonesProgressCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateZonesProgressCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class UpdateZonesProgressCommandHandler : CommandsHandler,
        IRequestHandler<UpdateZonesProgressCommand, Result>
    {
        public UpdateZonesProgressCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateZonesProgressCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateZonesProgressCommand request, CancellationToken token)
        {
            //var pointsPerRegions = await _context.RegionPoints.ToListAsync(token);
            //var pointsPerDepartments = await _context.DepartmentPoints.ToListAsync(token);

            var producersPerDepartments = await _context.DepartmentProducers.ToListAsync(token);
            var storesPerDepartments = await _context.DepartmentStores.ToListAsync(token);

            var departmentsPerRegions = producersPerDepartments.GroupBy(c => c.RegionId)
                .Select(c => new {Id = c.Key, DepartmentIds = c.Select(d => d.DepartmentId)});

            foreach (var region in departmentsPerRegions)
            {
                var producersCount = 0;
                var storesCount = 0;

                foreach (var departmentId in region.DepartmentIds)
                {
                    var producerPerDepartment = producersPerDepartments.First(p => p.DepartmentId == departmentId);
                    producersCount += (producerPerDepartment.Active ?? 0);

                    var storePerDepartment = storesPerDepartments.First(p => p.DepartmentId == departmentId);
                    storesCount += (storePerDepartment.Created ?? 0);

                    DepartmentPoints
                        pointsPerDepartment =
                            null; // pointsPerDepartments.FirstOrDefault(pp => pp.DepartmentId == departmentId);

                    _mediatr.Post(new UpdateDepartmentStatsCommand(request.RequestUser)
                    {
                        DepartmentId = departmentId,
                        Points = pointsPerDepartment?.Points ?? 0,
                        Position = pointsPerDepartment != null ? (int) pointsPerDepartment.Position : 0,
                        Producers = producerPerDepartment.Active ?? 0,
                        Stores = storePerDepartment.Created ?? 0
                    });
                }

                RegionPoints pointsPerRegion = null; //pointsPerRegions.FirstOrDefault(pp => pp.RegionId == region.Id);

                _mediatr.Post(new UpdateRegionStatsCommand(request.RequestUser)
                {
                    RegionId = region.Id,
                    Points = pointsPerRegion?.Points ?? 0,
                    Position = pointsPerRegion != null ? (int) pointsPerRegion.Position : 0,
                    Producers = producersCount,
                    Stores = storesCount
                });
            }

            return Success();
        }
    }
}