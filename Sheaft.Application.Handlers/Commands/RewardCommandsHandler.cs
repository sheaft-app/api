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

namespace Sheaft.Application.Handlers
{
    public class RewardCommandsHandler : CommandsHandler,
        IRequestHandler<CreateRewardCommand, Result<Guid>>,
        IRequestHandler<UpdateRewardCommand, Result<bool>>,
        IRequestHandler<DeleteRewardCommand, Result<bool>>,
        IRequestHandler<RestoreRewardCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;

        public RewardCommandsHandler(
            IAppDbContext context,
            ILogger<RewardCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Id == request.DepartmentId, token);
                var entity = new Reward(Guid.NewGuid(), request.Name, department);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetImage(request.Image);
                entity.SetEmail(request.Email);
                entity.SetPhone(request.Phone);
                entity.SetContact(request.Contact);
                entity.SetUrl(request.Url);

                var level = await _context.GetByIdAsync<Level>(request.LevelId, token);
                level.AddReward(entity);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Created(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Reward>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetImage(request.Image);
                entity.SetEmail(request.Email);
                entity.SetPhone(request.Phone);
                entity.SetContact(request.Contact);
                entity.SetUrl(request.Url);

                if (entity.Department.Id != request.DepartmentId)
                {
                    var department = await _context.Departments.SingleOrDefaultAsync(d => d.Id == request.DepartmentId, token);
                    entity.SetDepartment(department);
                }

                if (entity.Level.Id != request.LevelId)
                {
                    var oldLevel = await _context.GetByIdAsync<Level>(entity.Level.Id, token);
                    oldLevel.RemoveReward(entity);

                    var newLevel = await _context.GetByIdAsync<Level>(request.LevelId, token);
                    newLevel.AddReward(entity);
                }

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeleteRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Reward>(request.Id, token);
                entity.Remove();

                _context.Remove(entity);
                var results = await _context.SaveChangesAsync(token);

                return Ok(results > 0);
            });
        }

        public async Task<Result<bool>> Handle(RestoreRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.Rewards.SingleOrDefaultAsync(r => r.Id == request.Id, token);
                entity.Restore();

                _context.Update(entity);
                var results = await _context.SaveChangesAsync(token);

                return Ok(results > 0);
            });
        }
    }
}