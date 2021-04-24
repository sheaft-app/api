using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class CancelAgreementsCommand : Command
    {
        protected CancelAgreementsCommand()
        {
        }

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
                Result result = null;
                foreach (var agreementId in request.AgreementIds)
                {
                    result = await _mediatr.Process(
                        new CancelAgreementCommand(request.RequestUser) {AgreementId = agreementId, Reason = request.Reason},
                        token);

                    if (!result.Succeeded) break;
                }

                if (result is {Succeeded: false})
                    return result;

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}