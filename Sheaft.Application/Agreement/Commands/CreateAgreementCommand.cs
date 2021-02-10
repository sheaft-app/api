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
    public class CreateAgreementCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateAgreementCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
        public Guid DeliveryModeId { get; set; }
        public IEnumerable<TimeSlotGroupInput> SelectedHours { get; set; }
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
            return await ExecuteAsync(request, async() =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var store = await _context.GetByIdAsync<Store>(request.StoreId, token);
                var delivery = await _context.GetByIdAsync<DeliveryMode>(request.DeliveryModeId, token);

                var selectedHours = new List<TimeSlotHour>();
                if (request.SelectedHours != null && request.SelectedHours.Any())
                {
                    foreach (var sh in request.SelectedHours)
                    {
                        selectedHours.AddRange(sh.Days.Select(d => new TimeSlotHour(d, sh.From, sh.To)));
                    }
                }

                var entity = new Agreement(Guid.NewGuid(), store, delivery, user, selectedHours);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new AgreementCreatedEvent(request.RequestUser) { AgreementId = entity.Id });

                return Ok(entity.Id);
            });
        }
    }
}
