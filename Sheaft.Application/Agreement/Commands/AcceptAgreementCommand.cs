﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Agreement.Commands
{
    public class AcceptAgreementCommand : Command
    {
        [JsonConstructor]
        public AcceptAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }

    public class AcceptAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<AcceptAgreementCommand, Result>
    {
        public AcceptAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AcceptAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AcceptAgreementCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Agreement>(request.AgreementId, token);
            if(entity.CreatedBy.Kind == ProfileKind.Store && entity.Delivery.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if(entity.CreatedBy.Kind == ProfileKind.Producer && entity.Store.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.AcceptAgreement();

            var selectedHours = new List<TimeSlotHour>();
            if (request.SelectedHours != null && request.SelectedHours.Any())
            {
                foreach (var sh in request.SelectedHours)
                    selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));

                entity.SetSelectedHours(selectedHours);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}