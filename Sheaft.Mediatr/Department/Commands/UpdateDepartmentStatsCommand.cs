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

namespace Sheaft.Mediatr.Department.Commands
{
    public class UpdateDepartmentStatsCommand : Command
    {
        [JsonConstructor]
        public UpdateDepartmentStatsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DepartmentId { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }

    public class UpdateDepartmentStatsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDepartmentStatsCommand, Result>
    {
        public UpdateDepartmentStatsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDepartmentStatsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateDepartmentStatsCommand request, CancellationToken token)
        {
            var department = await _context.Departments.SingleOrDefaultAsync(c => c.Id == request.DepartmentId, token);
            var level = await _context.Levels
                .Where(c => c.RequiredPoints > request.Points)
                .OrderBy(c => c.RequiredPoints)
                .FirstOrDefaultAsync(token);

            department.SetLevel(level);
            department.SetPoints(request.Points);
            department.SetPosition(request.Position);
            var consumersCount = await _context.Users.OfType<Domain.Consumer>()
                .CountAsync(u => !u.RemovedOn.HasValue && u.Address.Department.Id == request.DepartmentId, token);

            department.SetConsumersCount(consumersCount);

            department.SetProducersCount(request.Producers);
            department.SetStoresCount(request.Stores);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}