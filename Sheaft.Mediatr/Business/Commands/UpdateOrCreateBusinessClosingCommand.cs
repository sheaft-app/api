﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Business.Commands
{
    public class UpdateOrCreateBusinessClosingCommand : Command<Guid>
    {
        protected UpdateOrCreateBusinessClosingCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateOrCreateBusinessClosingCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }
        public ClosingInputDto Closing { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = user.Id;
        }
    }

    public class UpdateOrCreateBusinessClosingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateBusinessClosingCommand, Result<Guid>>
    {
        public UpdateOrCreateBusinessClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateBusinessClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(UpdateOrCreateBusinessClosingCommand request, CancellationToken token)
        {
            var entity = await _context.Businesses.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure<Guid>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            Guid closingId;
            if (request.Closing.Id.HasValue)
            {
                var closing = entity.Closings.SingleOrDefault(c => c.Id == request.Closing.Id);
                if (closing == null)
                    return Failure<Guid>("Le créneau de fermeture est introuvable.");

                closing.ChangeClosedDates(request.Closing.From, request.Closing.To);
                closing.SetReason(request.Closing.Reason);
                closingId = closing.Id;
            }
            else
            {
                var closing = new Domain.BusinessClosing(entity, Guid.NewGuid(), request.Closing.From,
                    request.Closing.To, request.Closing.Reason);

                await _context.AddAsync(closing, token);
                
                entity.AddClosing(closing);
                closingId = closing.Id;
            }

            await _context.SaveChangesAsync(token);
            return Success(closingId);
        }
    }
}