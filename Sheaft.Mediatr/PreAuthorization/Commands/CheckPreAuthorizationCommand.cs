using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PreAuthorization;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CheckPreAuthorizationCommand : Command
    {
        [JsonConstructor]
        public CheckPreAuthorizationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PreAuthorizationId { get; set; }
    }

    public class CheckPreAuthorizationCommandHandler : CommandsHandler,
        IRequestHandler<CheckPreAuthorizationCommand, Result>
    {
        public CheckPreAuthorizationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPreAuthorizationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPreAuthorizationCommand request, CancellationToken token)
        {
            var preAuthorization = await _context.GetByIdAsync<Domain.PreAuthorization>(request.PreAuthorizationId, token);
            if (preAuthorization.Status == PreAuthorizationStatus.Succeeded && preAuthorization.PaymentStatus == PaymentStatus.Waiting)
            {
                var result = await _mediatr.Process(
                    new RefreshPreAuthorizationStatusCommand(request.RequestUser, preAuthorization.Identifier),
                    token);
                if (!result.Succeeded)
                    return Failure(result.Exception);
            }
            
            return Success();
        }
    }
}