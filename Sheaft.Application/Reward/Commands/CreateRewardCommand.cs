﻿using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CreateRewardCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

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
    
    public class CreateRewardCommandHandler : CommandsHandler,
        IRequestHandler<CreateRewardCommand, Result<Guid>>
    {
        public CreateRewardCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateRewardCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var department = await _context.Departments.SingleOrDefaultAsync(d => d.Id == request.DepartmentId, token);
                var entity = new Reward(Guid.NewGuid(), request.Name, department);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetPicture(request.Picture);
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
    }
}
