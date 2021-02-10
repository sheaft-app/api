using Sheaft.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class AcceptAgreementCommand : Command<bool>
    {
        [JsonConstructor]
        public AcceptAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
    }
    
    public class AcceptAgreementCommandsHandler : CommandsHandler,
        IRequestHandler<AcceptAgreementCommand, Result<bool>>
    {
        public AcceptAgreementCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<AcceptAgreementCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(AcceptAgreementCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async() =>
            {
                var entity = await _context.GetByIdAsync<Agreement>(request.Id, token);
                entity.AcceptAgreement();

                var selectedHours = new List<TimeSlotHour>();
                if (request.SelectedHours != null && request.SelectedHours.Any())
                {
                    foreach (var sh in request.SelectedHours)
                    {
                        selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));
                    }

                    entity.SetSelectedHours(selectedHours);
                }

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementAcceptedEvent(request.RequestUser) { AgreementId = entity.Id });
                return Ok(true);
            });
        }
    }
}
