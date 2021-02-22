using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Events.Agreement;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Agreement.Commands
{
    public class CancelAgreementCommand : Command
    {
        [JsonConstructor]
        public CancelAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public string Reason { get; set; }
    }

    public class CancelAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<CancelAgreementCommand, Result>
    {
        public CancelAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(entity.Delivery.Producer.Id != request.RequestUser.Id && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.CancelAgreement(request.Reason);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}