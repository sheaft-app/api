﻿using System;
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

namespace Sheaft.Mediatr.Setting.Commands
{
    public class DeleteSettingCommand : Command
    {
        protected DeleteSettingCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteSettingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid SettingId { get; set; }
    }

    public class DeleteSettingCommandHandler : CommandsHandler,
        IRequestHandler<DeleteSettingCommand, Result>
    {
        public DeleteSettingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteSettingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteSettingCommand request, CancellationToken token)
        {
            var entity = await _context.Settings.SingleAsync(e => e.Id == request.SettingId, token);
            _context.Remove(entity);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}