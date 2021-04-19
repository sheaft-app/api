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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class RemoveUserCommand : Command
    {
        [JsonConstructor]
        public RemoveUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Reason { get; set; }
    }

    public class RemoveUserCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserCommand, Result>
    {
        public RemoveUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            var hasActiveOrders = await _context.PurchaseOrders.AnyAsync(
                o => (o.Vendor.Id == entity.Id || o.Sender.Id == entity.Id) && (int) o.Status < 6, token);
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