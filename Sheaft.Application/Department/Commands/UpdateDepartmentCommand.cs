using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class UpdateDepartmentCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDepartmentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RequiredProducers { get; set; }
    }
    
    public class UpdateDepartmentCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDepartmentCommand, Result<bool>>
    {
        public UpdateDepartmentCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDepartmentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(UpdateDepartmentCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Departments.SingleOrDefaultAsync(c => c.Id == request.Id, token);

                entity.SetName(request.Name);
                entity.SetRequiredProducers(request.RequiredProducers);

                _context.Update(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
