using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
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
        private readonly RoleOptions _roles;

        public DeleteUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<DeleteUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _roles = roleOptions.Value;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id && !request.RequestUser.IsInAnyRoles(new []{_roles.Admin.Value, _roles.Support.Value}))
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var hasActiveOrders = await _context.PurchaseOrders.AnyAsync(
                o => (o.ProducerId == entity.Id || o.ClientId == entity.Id) && (int) o.Status < 6, token);
            if (hasActiveOrders)
                return Failure("Impossible de supprimer votre compte, vous avez des commandes en cours.");

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            _mediatr.Schedule(
                new RemoveUserDataCommand(request.RequestUser)
                    {UserId = request.UserId, Email = entity.Email, Reason = request.Reason}, TimeSpan.FromDays(14));
            return Success();
        }
    }
}