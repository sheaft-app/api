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
    public class RefuseAgreementCommand : Command<bool>
    {
        [JsonConstructor]
        public RefuseAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
    
    public class RefuseAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RefuseAgreementCommand, Result<bool>>
    {
        public RefuseAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RefuseAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RefuseAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);

                entity.RefuseAgreement(request.Reason);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementRefusedEvent(request.RequestUser) { AgreementId = entity.Id });
                return Ok(true);
            });
        }
    }
}
