using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class RestoreAgreementCommand : Command
    {
        [JsonConstructor]
        public RestoreAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class RestoreAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RestoreAgreementCommand, Result>
    {
        public RestoreAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreAgreementCommand request, CancellationToken token)
        {
            var entity =
                await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.AgreementId && a.RemovedOn.HasValue, token);
            if(entity.Delivery.Producer.Id != request.RequestUser.Id && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}