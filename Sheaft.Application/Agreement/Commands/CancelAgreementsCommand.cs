using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CancelAgreementsCommand : Command<bool>
    {
        [JsonConstructor]
        public CancelAgreementsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
    
    public class CancelAgreementsCommandsHandler : CommandsHandler,
        IRequestHandler<CancelAgreementsCommand, Result<bool>>
    {
        public CancelAgreementsCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelAgreementsCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CancelAgreementsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var agreementId in request.Ids)
                    {
                        var result = await _mediatr.Process(new CancelAgreementCommand(request.RequestUser) { Id = agreementId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }
    }
}
