using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreAgreementCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestoreAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RestoreAgreementCommand, Result<bool>>
    {
        public RestoreAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RestoreAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                _context.Restore(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
