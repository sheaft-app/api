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
    public class UpdateDepartmentStatsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDepartmentStatsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int Producers { get; set; }
        public int Stores { get; set; }
    }
    
    public class UpdateDepartmentStatsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDepartmentStatsCommand, Result<bool>>
    {
        public UpdateDepartmentStatsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDepartmentStatsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateDepartmentStatsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
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

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
