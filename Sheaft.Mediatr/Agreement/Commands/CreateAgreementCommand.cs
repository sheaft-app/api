﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
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
        [JsonConstructor]
        public CreateAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public IEnumerable<TimeSlotGroupDto> SelectedHours { get; set; }
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
            var store = await _context.GetByIdAsync<Domain.Store>(request.StoreId, token);
            var currentUser = await _context.GetByIdAsync<Domain.User>(request.RequestUser.Id, token);
            var delivery = await _context.GetByIdAsync<Domain.DeliveryMode>(request.DeliveryModeId, token);

            var selectedHours = new List<TimeSlotHour>();
            if (request.SelectedHours != null && request.SelectedHours.Any())
            {
                foreach (var sh in request.SelectedHours)
                {
                    selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));
                }
            }

            var entity = new Domain.Agreement(Guid.NewGuid(), store, delivery, currentUser, selectedHours);
            if (request.CatalogId.HasValue)
            {
                var catalog = await _context.GetByIdAsync<Domain.Catalog>(request.CatalogId.Value, token);
                entity.AssignCatalog(catalog);
            }
            else
            {
                var catalog = await _context.GetSingleAsync<Domain.Catalog>(c => c.IsDefault && c.Kind == CatalogKind.Stores && c.Producer.Id == delivery.Producer.Id, token);
                entity.AssignCatalog(catalog);
            }

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            return Success(entity.Id);
        }
    }
}