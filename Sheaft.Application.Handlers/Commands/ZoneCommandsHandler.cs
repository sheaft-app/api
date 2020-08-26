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
                var users = await _context.Users.CountAsync(u => !u.RemovedOn.HasValue && u.UserType == Interop.Enums.UserKind.Consumer && u.Department != null && u.Department.Region != null && u.Department.Region.Id == request.Id, token);

                region.SetConsumersCount(users);

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
                var users = await _context.Users.CountAsync(u => !u.RemovedOn.HasValue && u.UserType == Interop.Enums.UserKind.Consumer && u.Department != null && u.Department.Id == request.Id, token);

                department.SetConsumersCount(users);

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

                        var pointsPerDepartment = pointsPerDepartments.FirstOrDefault(pp => pp.DepartmentId == departmentId);

                        await _queueService.ProcessCommandAsync(UpdateDepartmentCommand.QUEUE_NAME, new UpdateDepartmentCommand(request.RequestUser) 
                            {
                                Id = departmentId,
                                Points = pointsPerDepartment?.Points ?? 0,
                                Position = (int)pointsPerDepartment?.Position,
                                Producers = producerPerDepartment.Created ?? 0,
                                Stores = storePerDepartment.Created ?? 0
                        }, token);
                    }

                    var pointsPerRegion = pointsPerRegions.FirstOrDefault(pp => pp.RegionId == region.Id);

                    await _queueService.ProcessCommandAsync(UpdateRegionCommand.QUEUE_NAME, new UpdateRegionCommand(request.RequestUser)
                    {
                        Id = region.Id,
                        Points = pointsPerRegion?.Points ?? 0,
                        Position = (int)pointsPerRegion?.Position,
                        Producers = producersCount,
                        Stores = storesCount
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
                    ProducersRequired = d.RequiredProducers,
                    ConsumersCount = d.ConsumersCount,
                    StoresCount = d.StoresCount

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
            public int ConsumersCount { get; set; }
            public int StoresCount { get; set; }
        }
    }
}