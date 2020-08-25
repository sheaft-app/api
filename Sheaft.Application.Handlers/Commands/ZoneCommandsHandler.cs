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
using Sheaft.Services.Interop;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Sheaft.Application.Handlers
{
    public class ZoneCommandsHandler : CommandsHandler,
        IRequestHandler<UpdateZoneProgressCommand, CommandResult<bool>>,
        IRequestHandler<GenerateZonesFileCommand, CommandResult<bool>>,
        IRequestHandler<UpdateDepartmentCommand, CommandResult<bool>>,
        IRequestHandler<UpdateRegionCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IQueueService _queueService;
        private readonly IBlobService _blobService;

        public ZoneCommandsHandler(
            IAppDbContext context,
            IQueueService queueService,
            IBlobService blobService,
            ILogger<ZoneCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _queueService = queueService;
            _blobService = blobService;
        }

        public async Task<CommandResult<bool>> Handle(UpdateRegionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var region = await _context.GetByIdAsync<Region>(request.Id, token);

                region.SetPoints(request.Points);
                region.SetPosition(request.Position);
                region.SetConsumersCount(request.Users);

                region.SetProducersCount(request.Producers);
                region.SetStoresCount(request.Stores);

                _context.Update(region);
                await _context.SaveChangesAsync(token);

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateDepartmentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var department = await _context.GetByIdAsync<Department>(request.Id, token);
                var level = (await _context.GetAsync<Level>(c => c.RequiredPoints > request.Points, token)).OrderBy(c => c.RequiredPoints).FirstOrDefault();

                department.SetLevel(level);
                department.SetPoints(request.Points);
                department.SetPosition(request.Position);
                department.SetConsumersCount(request.Users);

                department.SetProducersCount(request.Producers);
                department.SetStoresCount(request.Stores);

                _context.Update(department);
                await _context.SaveChangesAsync(token);

                return OkResult(true);
            });
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
                    var producersCount = 0;
                    var storesCount = 0;

                    foreach (var pointsPerDepartment in pointsPerDepartments.Where(d => d.RegionId == pointsPerRegion.RegionId))
                    { 
                        var producerCount = producersPerDepartments.FirstOrDefault(p => p.DepartmentCode == pointsPerDepartment.Code);
                        if (producerCount != null)
                            producersCount += (producerCount.Created ?? 0);

                        var storeCount = storesPerDepartments.FirstOrDefault(p => p.DepartmentCode == pointsPerDepartment.Code);
                        if (storeCount != null)
                            storesCount += (storeCount.Created ?? 0);

                        await _queueService.ProcessCommandAsync(UpdateDepartmentCommand.QUEUE_NAME, new UpdateDepartmentCommand(request.RequestUser) 
                            {
                                Id = pointsPerDepartment.DepartmentId,
                                Points = pointsPerDepartment.Points ?? 0,
                                Position = (int)pointsPerDepartment.Position,
                                Producers = producerCount.Created ?? 0,
                                Stores = storeCount.Created ?? 0,
                                Users = pointsPerDepartment.Users
                        }, token);
                    }

                    await _queueService.ProcessCommandAsync(UpdateRegionCommand.QUEUE_NAME, new UpdateRegionCommand(request.RequestUser)
                    {
                        Id = pointsPerRegion.RegionId,
                        Points = pointsPerRegion.Points ?? 0,
                        Position = (int)pointsPerRegion.Position,
                        Producers = producersCount,
                        Stores = storesCount,
                        Users = pointsPerRegion.Users
                    }, token);
                }

                return OkResult(true);
            });
        }

        public async Task<CommandResult<bool>> Handle(GenerateZonesFileCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var departments = await _context.Departments.Where(d => !d.RemovedOn.HasValue).ToListAsync(token);
                var depts = departments.Select(d => new DepartmentProgress
                {
                    Code = d.Code,
                    Name = d.Name,
                    Points = d.Points,
                    Position = d.Position,
                    ProducersCount = d.ProducersCount,
                    ProducersRequired = d.RequiredProducers
                }).ToList();

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(depts))))
                {
                    await _blobService.UploadDepartmentsProgressAsync(stream, token);
                }

                return OkResult(true);
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
        }
    }
}