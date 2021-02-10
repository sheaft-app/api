using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RemoveUserCommand : Command<bool>
    {
        [JsonConstructor]
        public RemoveUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
    
    public class RemoveUserCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public RemoveUserCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RemoveUserCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
        }
        
        public async Task<Result<bool>> Handle(RemoveUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                var hasActiveOrders = await _context.AnyAsync<PurchaseOrder>(o => (o.Vendor.Id == entity.Id || o.Sender.Id == entity.Id) && (int)o.Status < 6, token);
                if (hasActiveOrders)
                    return ValidationError<bool>(MessageKind.Consumer_CannotBeDeleted_HasActiveOrders);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                _mediatr.Schedule(new RemoveUserDataCommand(request.RequestUser) { Id = request.Id, Email = entity.Email, Reason = request.Reason }, TimeSpan.FromDays(14));
                return Ok(true);
            });
        }
    }
}
