using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class CreateAgreementCommand : Command<Guid>
    {
        protected CreateAgreementCommand()
        {
        }
        
        [JsonConstructor]
        public CreateAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
        public Guid ProducerId { get; set; }
        public Guid? DeliveryId { get; set; }
        public Guid? CatalogId { get; set; }
    }

    public class CreateAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<CreateAgreementCommand, Result<Guid>>
    {
        public CreateAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateAgreementCommand request, CancellationToken token)
        {
            var store = await _context.Stores.SingleAsync(e => e.Id == request.StoreId, token);
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            var currentUser = await _context.Users.SingleAsync(e => e.Id == request.RequestUser.Id, token);

            Domain.DeliveryMode delivery = null;
            if (request.DeliveryId.HasValue)
                delivery = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryId, token);
            
            var entity = new Domain.Agreement(Guid.NewGuid(), store, producer, currentUser.Kind, delivery);
            if (request.CatalogId.HasValue)
            {
                var catalog = await _context.Catalogs.SingleAsync(e => e.Id == request.CatalogId.Value, token);
                entity.AssignCatalog(catalog);
            }
            else
            {
                var catalog = await _context.Catalogs.SingleAsync(c => c.IsDefault && c.Kind == CatalogKind.Stores && c.ProducerId == producer.Id, token);
                entity.AssignCatalog(catalog);
            }

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            return Success(entity.Id);
        }
    }
}
