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
    public class RefuseAgreementsCommand : Command<bool>
    {
        [JsonConstructor]
        public RefuseAgreementsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
    
    public class RefuseAgreementsCommandsHandler : CommandsHandler,
        IRequestHandler<RefuseAgreementsCommand, Result<bool>>
    {
        public RefuseAgreementsCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RefuseAgreementsCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(RefuseAgreementsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var agreementId in request.Ids)
                    {
                        var result = await _mediatr.Process(new RefuseAgreementCommand(request.RequestUser) { Id = agreementId, Reason = request.Reason }, token);
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
