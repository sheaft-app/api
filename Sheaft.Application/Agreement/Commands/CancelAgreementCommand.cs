using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CancelAgreementCommand : Command<bool>
    {
        [JsonConstructor]
        public CancelAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
    
    public class CancelAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<CancelAgreementCommand, Result<bool>>
    {
        public CancelAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CancelAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.CancelAgreement(request.Reason);

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementCancelledEvent(request.RequestUser) { AgreementId = entity.Id });
                return Ok(true);
            });
        }
    }
}
