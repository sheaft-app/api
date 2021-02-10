using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Department.Commands
{
    public class UpdateDepartmentCommand : Command
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
            var entity = await _context.Departments.SingleOrDefaultAsync(c => c.Id == request.Id, token);

            entity.SetName(request.Name);
            entity.SetRequiredProducers(request.RequiredProducers);

            _context.Update(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}