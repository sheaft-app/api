using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryClosing.Commands
{
    public class UpdateOrCreateDeliveryClosingsCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public UpdateOrCreateDeliveryClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public IEnumerable<UpdateOrCreateClosingDto> Closings { get; set; }
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
                var entity = await _context.GetByIdAsync<Domain.DeliveryMode>(request.DeliveryId, token);
                var existingClosingIds = entity.Closings.Select(c => c.Id).ToList();
                
                var closingIdsToRemove = existingClosingIds.Except(request.Closings?.Where(c => c.Id.HasValue)?.Select(c => c.Id.Value) ?? new List<Guid>()).ToList();
                if (closingIdsToRemove.Any())
                {
                    entity.RemoveClosings(closingIdsToRemove);
                    await _context.SaveChangesAsync(token);
                }

                var ids = new List<Guid>();
                if (request.Closings != null)
                {
                    foreach (var closing in request.Closings)
                    {
                        var result =
                            await _mediatr.Process(
                                new UpdateOrCreateDeliveryClosingCommand(request.RequestUser)
                                    {DeliveryId = request.DeliveryId, Closing = closing}, token);

                        if (!result.Succeeded)
                            throw result.Exception;

                        ids.Add(result.Data);
                    }
                }

                await transaction.CommitAsync(token);
                return Success<IEnumerable<Guid>>(ids);
            }
        }
    }
}