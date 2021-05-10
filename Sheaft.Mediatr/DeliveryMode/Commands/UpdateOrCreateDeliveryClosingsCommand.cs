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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryClosing.Commands
{
    public class UpdateOrCreateDeliveryClosingsCommand : Command<IEnumerable<Guid>>
    {
        protected UpdateOrCreateDeliveryClosingsCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateOrCreateDeliveryClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public IEnumerable<ClosingInputDto> Closings { get; set; }
    }

    public class UpdateOrCreateDeliveryClosingsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateDeliveryClosingsCommand, Result<IEnumerable<Guid>>>
    {
        public UpdateOrCreateDeliveryClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateDeliveryClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(UpdateOrCreateDeliveryClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryId, token);
                var existingClosingIds = entity.Closings?.Select(c => c.Id)?.ToList() ?? new List<Guid>();

                var requestClosings = request.Closings?.ToList() ?? new List<ClosingInputDto>();
                var closingIdsToRemove = existingClosingIds.Except(requestClosings.Where(c => c.Id.HasValue)?.Select(c => c.Id.Value) ?? new List<Guid>()).ToList();
                if (closingIdsToRemove.Any())
                {
                    entity.RemoveClosings(closingIdsToRemove);
                    await _context.SaveChangesAsync(token);
                }

                var ids = new List<Result<Guid>>();
                foreach (var closing in requestClosings)
                {
                    var result =
                        await _mediatr.Process(
                            new UpdateOrCreateDeliveryClosingCommand(request.RequestUser)
                                {DeliveryId = request.DeliveryId, Closing = closing}, token);

                    ids.Add(result);
                }

                if (ids.Any(r => !r.Succeeded))
                    return Failure<IEnumerable<Guid>>(ids.First(r => !r.Succeeded));

                await transaction.CommitAsync(token);
                return Success<IEnumerable<Guid>>(ids.Select(r => r.Data));
            }
        }
    }
}
