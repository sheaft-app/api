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

namespace Sheaft.Services.Agreement.Commands
{
    public class ResetAgreementStatusToCommand : Command
    {
        [JsonConstructor]
        public ResetAgreementStatusToCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class ResetAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<ResetAgreementStatusToCommand, Result>
    {
        public ResetAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ResetAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ResetAgreementStatusToCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.AgreementId, token);
            if(entity.Delivery.Producer.Id != request.RequestUser.Id && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.Reset();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}