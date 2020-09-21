using Sheaft.Application.Interop;
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
    public class DepartmentCommandsHandler : ResultsHandler,
        IRequestHandler<UpdateDepartmentCommand, Result<bool>>,
        IRequestHandler<UpdateDepartmentStatsCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;

        public DepartmentCommandsHandler(
            IAppDbContext context,
            ILogger<DepartmentCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<bool>> Handle(UpdateDepartmentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Departments.SingleOrDefaultAsync(c => c.Id == request.Id, token);

                entity.SetName(request.Name);
                entity.SetRequiredProducers(request.RequiredProducers);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(UpdateDepartmentStatsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var department = await _context.Departments.SingleOrDefaultAsync(c => c.Id == request.Id, token);
                var level = (await _context.GetAsync<Level>(c => c.RequiredPoints > request.Points, token)).OrderBy(c => c.RequiredPoints).FirstOrDefault();

                department.SetLevel(level);
                department.SetPoints(request.Points);
                department.SetPosition(request.Position);
                var consumersCount = await _context.Users.OfType<Consumer>().CountAsync(u => !u.RemovedOn.HasValue && u.Address.Department.Id == request.Id, token);

                department.SetConsumersCount(consumersCount);

                department.SetProducersCount(request.Producers);
                department.SetStoresCount(request.Stores);

                _context.Update(department);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}