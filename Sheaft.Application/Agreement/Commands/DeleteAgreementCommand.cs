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
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Agreement.Commands
{
    public class DeleteAgreementCommand : Command
    {
        [JsonConstructor]
        public DeleteAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }

    public class DeleteAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<DeleteAgreementCommand, Result>
    {
        public DeleteAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(entity.Delivery.Producer.Id != request.RequestUser.Id && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}