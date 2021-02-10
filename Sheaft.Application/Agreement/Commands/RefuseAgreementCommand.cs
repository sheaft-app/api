using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Events.Agreement;

namespace Sheaft.Application.Agreement.Commands
{
    public class RefuseAgreementCommand : Command
    {
        [JsonConstructor]
        public RefuseAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }

    public class RefuseAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RefuseAgreementCommand, Result>
    {
        public RefuseAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RefuseAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RefuseAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.Id, token);
            entity.RefuseAgreement(request.Reason);
            
            await _context.SaveChangesAsync(token);
            return Success(true);
        }
    }
}