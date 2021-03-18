using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Services.Agreement.Commands
{
    public class RefuseAgreementCommand : Command
    {
        [JsonConstructor]
        public RefuseAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
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
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(entity.CreatedBy.Kind == ProfileKind.Store && entity.Delivery.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if(entity.CreatedBy.Kind == ProfileKind.Producer && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.RefuseAgreement(request.Reason);
            
            await _context.SaveChangesAsync(token);
            return Success(true);
        }
    }
}