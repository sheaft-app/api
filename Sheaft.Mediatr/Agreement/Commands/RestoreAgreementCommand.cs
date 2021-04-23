using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class RestoreAgreementCommand : Command
    {
        protected RestoreAgreementCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class RestoreAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<RestoreAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public RestoreAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<RestoreAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(RestoreAgreementCommand request, CancellationToken token)
        {
            var entity =
                await _context.Agreements.SingleOrDefaultAsync(a => a.Id == request.AgreementId && a.RemovedOn.HasValue, token);
            
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.Store.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}