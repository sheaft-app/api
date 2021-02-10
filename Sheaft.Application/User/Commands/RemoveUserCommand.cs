using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.User.Commands
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
            var entity = await _context.GetByIdAsync<Domain.User>(request.UserId, token);

            var hasActiveOrders = await _context.AnyAsync<Domain.PurchaseOrder>(
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