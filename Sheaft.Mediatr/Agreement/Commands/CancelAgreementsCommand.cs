using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class CancelAgreementsCommand : Command
    {
        [JsonConstructor]
        public CancelAgreementsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> AgreementIds { get; set; }
        public string Reason { get; set; }
    }

    public class CancelAgreementsCommandsHandler : CommandsHandler,
        IRequestHandler<CancelAgreementsCommand, Result>
    {
        public CancelAgreementsCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelAgreementsCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelAgreementsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var agreementId in request.AgreementIds)
                {
                    var result = await _mediatr.Process(
                        new CancelAgreementCommand(request.RequestUser) {AgreementId = agreementId, Reason = request.Reason},
                        token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}