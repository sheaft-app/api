using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Agreement.Commands
{
    public class ResetAgreementStatusToCommand : Command
    {
        [JsonConstructor]
        public ResetAgreementStatusToCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class ResetAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<ResetAgreementStatusToCommand, Result>
    {
        public ResetAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<ResetAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ResetAgreementStatusToCommand request, CancellationToken token)
        {
            var entity = await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.AgreementId, token);

            entity.Reset();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}