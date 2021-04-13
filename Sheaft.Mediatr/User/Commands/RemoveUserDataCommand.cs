using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;

namespace Sheaft.Mediatr.User.Commands
{
    public class RemoveUserDataCommand : Command<string>
    {
        [JsonConstructor]
        public RemoveUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
    }

    public class RemoveUserDataCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserDataCommand, Result<string>>
    {
        private readonly IBlobService _blobService;

        public RemoveUserDataCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RemoveUserDataCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result<string>> Handle(RemoveUserDataCommand request, CancellationToken token)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.UserId, token);
            if (entity == null)
                return Failure<string>(MessageKind.NotFound);

            if (!entity.RemovedOn.HasValue)
                return Success(request.Reason);
            
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            await _blobService.CleanUserStorageAsync(request.UserId, token);

            var result = await _mediatr.Process(new RemoveAuthUserCommand(request.RequestUser)
            {
                UserId = entity.Id
            }, token);

            if (!result.Succeeded)
                return Failure<string>(result.Exception);

            entity.Close();
            await _context.SaveChangesAsync(token);

            return Success(request.Reason);
        }
    }
}