using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Handlers
{
    public class ZoneCommandsHandler : ResultsHandler,
        IRequestHandler<UpdateZoneProgressCommand, Result<bool>>,
        IRequestHandler<GenerateZonesFileCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;

        public ZoneCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ZoneCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(UpdateZoneProgressCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                //var pointsPerRegions = await _context.RegionPoints.ToListAsync(token);
                //var pointsPerDepartments = await _context.DepartmentPoints.ToListAsync(token);

                var producersPerDepartments = await _context.DepartmentProducers.ToListAsync(token);
                var storesPerDepartments = await _context.DepartmentStores.ToListAsync(token);

                var departmentsPerRegions = producersPerDepartments.GroupBy(c => c.RegionId).Select(c => new { Id = c.Key, DepartmentIds = c.Select(d => d.DepartmentId) });

                foreach (var region in departmentsPerRegions)
                {
                    var producersCount = 0;
                    var storesCount = 0;

                    foreach (var departmentId in region.DepartmentIds)
                    {
                        var producerPerDepartment = producersPerDepartments.First(p => p.DepartmentId == departmentId);
                        producersCount += (producerPerDepartment.Created ?? 0);

                        var storePerDepartment = storesPerDepartments.First(p => p.DepartmentId == departmentId);
                        storesCount += (storePerDepartment.Created ?? 0);

                        DepartmentPoints pointsPerDepartment = null;// pointsPerDepartments.FirstOrDefault(pp => pp.DepartmentId == departmentId);

                        await _mediatr.Post(new UpdateDepartmentStatsCommand(request.RequestUser) 
                            {
                                Id = departmentId,
                                Points = pointsPerDepartment?.Points ?? 0,
                                Position = pointsPerDepartment != null ? (int)pointsPerDepartment.Position : 0,
                                Producers = producerPerDepartment.Created ?? 0,
                                Stores = storePerDepartment.Created ?? 0
                        }, token);
                    }

                    RegionPoints pointsPerRegion = null;//pointsPerRegions.FirstOrDefault(pp => pp.RegionId == region.Id);

                    await _mediatr.Post(new UpdateRegionStatsCommand(request.RequestUser)
                    {
                        Id = region.Id,
                        Points = pointsPerRegion?.Points ?? 0,
                        Position = pointsPerRegion != null ? (int)pointsPerRegion.Position : 0,
                        Producers = producersCount,
                        Stores = storesCount
                    }, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(GenerateZonesFileCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var departments = await _context.Departments.ToListAsync(token);
                var depts = departments.Select(d => new DepartmentProgress
                {
                    Code = d.Code,
                    Name = d.Name,
                    Points = d.Points,
                    Position = d.Position,
                    ProducersCount = d.ProducersCount,
                    ProducersRequired = d.RequiredProducers,
                    ConsumersCount = d.ConsumersCount,
                    StoresCount = d.StoresCount
                }).ToList();

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(depts))))
                {
                    await _blobService.UploadDepartmentsProgressAsync(stream, token);
                }

                return Ok(true);
            });
        }

        internal class DepartmentProgress
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int Position { get; set; }
            public int Points { get; set; }
            public int ProducersCount { get; set; }
            public int? ProducersRequired { get; set; }
            public int ConsumersCount { get; set; }
            public int StoresCount { get; set; }
        }
    }
}