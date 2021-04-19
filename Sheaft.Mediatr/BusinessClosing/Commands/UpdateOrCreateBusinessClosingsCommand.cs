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
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.BusinessClosing.Commands
{
    public class UpdateOrCreateBusinessClosingsCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public UpdateOrCreateBusinessClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public IEnumerable<UpdateOrCreateClosingDto> Closings { get; set; }
    }

    public class UpdateOrCreateBusinessClosingsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateBusinessClosingsCommand, Result<IEnumerable<Guid>>>
    {
        public UpdateOrCreateBusinessClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateBusinessClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(UpdateOrCreateBusinessClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var entity = await _context.Businesses.SingleAsync(e => e.Id == request.UserId, token);
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
                                new UpdateOrCreateBusinessClosingCommand(request.RequestUser)
                                    {UserId = request.UserId, Closing = closing}, token);

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