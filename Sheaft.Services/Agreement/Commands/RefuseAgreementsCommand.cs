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

namespace Sheaft.Services.Agreement.Commands
{
    public class RefuseAgreementsCommand : Command
    {
        [JsonConstructor]
        public RefuseAgreementsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> AgreementIds { get; set; }
        public string Reason { get; set; }
    }

    public class RefuseAgreementsCommandsHandler : CommandsHandler,
        IRequestHandler<RefuseAgreementsCommand, Result>
    {
        public RefuseAgreementsCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RefuseAgreementsCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RefuseAgreementsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var agreementId in request.AgreementIds)
                {
                    var result = await _mediatr.Process(
                        new RefuseAgreementCommand(request.RequestUser) {AgreementId = agreementId, Reason = request.Reason},
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