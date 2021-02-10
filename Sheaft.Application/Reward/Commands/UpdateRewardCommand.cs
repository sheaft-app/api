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

namespace Sheaft.Application.Reward.Commands
{
    public class UpdateRewardCommand : Command
    {
        [JsonConstructor]
        public UpdateRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }
        public string Url { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid LevelId { get; set; }
    }

    public class UpdateRewardCommandHandler : CommandsHandler,
        IRequestHandler<UpdateRewardCommand, Result>
    {
        public UpdateRewardCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateRewardCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateRewardCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Reward>(request.Id, token);

            entity.SetName(request.Name);
            entity.SetDescription(request.Description);
            entity.SetPicture(request.Picture);
            entity.SetEmail(request.Email);
            entity.SetPhone(request.Phone);
            entity.SetContact(request.Contact);
            entity.SetUrl(request.Url);

            if (entity.Department.Id != request.DepartmentId)
            {
                var department =
                    await _context.Departments.SingleOrDefaultAsync(d => d.Id == request.DepartmentId, token);
                entity.SetDepartment(department);
            }

            if (entity.Level.Id != request.LevelId)
            {
                var oldLevel = await _context.GetByIdAsync<Domain.Level>(entity.Level.Id, token);
                oldLevel.RemoveReward(entity);

                var newLevel = await _context.GetByIdAsync<Domain.Level>(request.LevelId, token);
                newLevel.AddReward(entity);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}