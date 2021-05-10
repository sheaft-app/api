using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class DeleteUserCommand : Command
    {
        protected DeleteUserCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteUserCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }
        public string Reason { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class DeleteUserCommandHandler : CommandsHandler,
        IRequestHandler<DeleteUserCommand, Result>
    {
        public DeleteUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            var hasActiveOrders = await _context.PurchaseOrders.AnyAsync(
                o => (o.ProducerId == entity.Id || o.ClientId == entity.Id) && (int) o.Status < 6, token);
            if (hasActiveOrders)
                return Failure(MessageKind.Consumer_CannotBeDeleted_HasActiveOrders);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            _mediatr.Schedule(
                new RemoveUserDataCommand(request.RequestUser)
                    {UserId = request.UserId, Email = entity.Email, Reason = request.Reason}, TimeSpan.FromDays(14));
            return Success();
        }
    }
}