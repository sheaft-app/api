using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Agreement.Commands
{
    public class AcceptAgreementCommand : Command
    {
        [JsonConstructor]
        public AcceptAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public Guid? CatalogId { get; set; }
        public IEnumerable<TimeSlotGroupDto> SelectedHours { get; set; }
    }

    public class AcceptAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<AcceptAgreementCommand, Result>
    {
        private readonly RoleOptions _roleOptions;

        public AcceptAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<AcceptAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(AcceptAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(request.RequestUser.IsInRole(_roleOptions.Producer.Value) && entity.Delivery.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            if(request.RequestUser.IsInRole(_roleOptions.Store.Value) && entity.Store.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.AcceptAgreement();

            var selectedHours = new List<TimeSlotHour>();
            if (request.SelectedHours != null && request.SelectedHours.Any())
            {
                foreach (var sh in request.SelectedHours)
                    selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));

                entity.SetSelectedHours(selectedHours);
            }
            
            if (request.CatalogId.HasValue && entity.Catalog?.Id != request.CatalogId.Value)
            {
                var catalog = await _context.GetByIdAsync<Domain.Catalog>(request.CatalogId.Value, token);
                entity.AssignCatalog(catalog);
            }
            else if (!request.CatalogId.HasValue && entity.Catalog?.Id == null)
            {
                var catalog = await _context.GetSingleAsync<Domain.Catalog>(c => c.IsDefault && c.Kind == CatalogKind.Stores && c.Producer.Id == entity.Delivery.Producer.Id, token);
                entity.AssignCatalog(catalog);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}