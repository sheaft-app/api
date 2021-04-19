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
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CheckPayinCommand : Command
    {
        protected CheckPayinCommand()
        {
            
        }
        [JsonConstructor]
        public CheckPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }

    public class CheckPayinCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinCommand, Result>
    {
        public CheckPayinCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayinCommand request, CancellationToken token)
        {
            var payin = await _context.Payins.SingleAsync(e => e.Id == request.PayinId, token);
            if (payin.Status == TransactionStatus.Created || payin.Status == TransactionStatus.Waiting)
            {
                var result = await _mediatr.Process(
                    new RefreshPayinStatusCommand(request.RequestUser, payin.Identifier),
                    token);
                if (!result.Succeeded)
                    return Failure(result);
            }
            
            return Success();
        }
    }
}