using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class AcceptAgreementsCommand : Command
    {
        protected AcceptAgreementsCommand()
        {
        }

        [JsonConstructor]
        public AcceptAgreementsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> AgreementIds { get; set; }
        public Guid? CatalogId { get; set; }
        public Guid? DeliveryId { get; set; }
    }

    public class AcceptAgreementsCommandsHandler : CommandsHandler,
        IRequestHandler<AcceptAgreementsCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public AcceptAgreementsCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<AcceptAgreementsCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(AcceptAgreementsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var agreementId in request.AgreementIds)
                {
                    var result = await _mediatr.Process(
                        new AcceptAgreementCommand(request.RequestUser)
                        {
                            AgreementId = agreementId, 
                            CatalogId = request.CatalogId,
                            DeliveryId = request.DeliveryId
                        }, token);
                    
                    if (!result.Succeeded)
                        return Failure(result);
                }

                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}