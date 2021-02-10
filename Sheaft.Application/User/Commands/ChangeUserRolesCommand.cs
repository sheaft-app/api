using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class ChangeUserRolesCommand : Command<bool>
    {
        [JsonConstructor]
        public ChangeUserRolesCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
    
    public class ChangeUserRolesCommandHandler : CommandsHandler,
        IRequestHandler<ChangeUserRolesCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public ChangeUserRolesCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<ChangeUserRolesCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
        }

        public async Task<Result<bool>> Handle(ChangeUserRolesCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.UserId, token);

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

                if (!result.Success)
                    return result;

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
