using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class ResetAgreementStatusToCommand : Command<bool>
    {
        [JsonConstructor]
        public ResetAgreementStatusToCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class ResetAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<ResetAgreementStatusToCommand, Result<bool>>
    {
        public ResetAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ResetAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(ResetAgreementStatusToCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.Id, token);

                entity.Reset();
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
