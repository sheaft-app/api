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
using System.Linq;

namespace Sheaft.Application.Handlers
{
    public class DepartmentCommandsHandler : CommandsHandler,
        IRequestHandler<UpdateDepartmentCommand, CommandResult<bool>>,
        IRequestHandler<UpdateDepartmentStatsCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;

        public DepartmentCommandsHandler(
            IAppDbContext context,
            ILogger<DepartmentCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<CommandResult<bool>> Handle(UpdateDepartmentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Department>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetRequiredProducers(request.RequiredProducers);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateDepartmentStatsCommand request, CancellationToken token)
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

                return Ok(true);
            });
        }
    }
}