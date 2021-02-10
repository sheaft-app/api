using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Auth.Commands;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.User.Commands
{
    public class ChangeUserRolesCommand : Command
    {
        [JsonConstructor]
        public ChangeUserRolesCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class ChangeUserRolesCommandHandler : CommandsHandler,
        IRequestHandler<ChangeUserRolesCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public ChangeUserRolesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ChangeUserRolesCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(ChangeUserRolesCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            var roles = new List<Guid>();
            if (request.Roles.Contains(_roleOptions.Producer.Value))
            {
                roles.Add(_roleOptions.Producer.Id);
            }

            if (request.Roles.Contains(_roleOptions.Store.Value))
            {
                roles.Add(_roleOptions.Owner.Id);
                roles.Add(_roleOptions.Store.Id);
            }

            if (request.Roles.Contains(_roleOptions.Consumer.Value))
            {
                roles.Add(_roleOptions.Owner.Id);
                roles.Add(_roleOptions.Consumer.Id);
            }

            var result = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Name = entity.Name,
                Phone = entity.Phone,
                Picture = entity.Picture,
                Roles = roles,
                UserId = entity.Id
            }, token);

            if (!result.Succeeded)
                return result;

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}