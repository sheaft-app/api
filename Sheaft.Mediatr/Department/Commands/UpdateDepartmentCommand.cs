using System;
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

namespace Sheaft.Mediatr.Department.Commands
{
    public class UpdateDepartmentCommand : Command
    {
        protected UpdateDepartmentCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateDepartmentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public int? RequiredProducers { get; set; }
    }

    public class UpdateDepartmentCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDepartmentCommand, Result>
    {
        public UpdateDepartmentCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDepartmentCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken token)
        {
            var entity = await _context.Departments.SingleOrDefaultAsync(c => c.Id == request.DepartmentId, token);

            entity.SetName(request.Name);
            entity.SetRequiredProducers(request.RequiredProducers);

            _context.Update(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}