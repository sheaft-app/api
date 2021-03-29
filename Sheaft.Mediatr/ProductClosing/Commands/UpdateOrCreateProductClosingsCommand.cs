using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.ProductClosing.Commands
{
    public class UpdateOrCreateProductClosingsCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public UpdateOrCreateProductClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public IEnumerable<UpdateOrCreateClosingDto> Closings { get; set; }
    }

    public class UpdateOrCreateProductClosingsCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateProductClosingsCommand, Result<IEnumerable<Guid>>>
    {
        public UpdateOrCreateProductClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateProductClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(UpdateOrCreateProductClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var product = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
                var existingClosingIds = product.Closings.Select(c => c.Id).ToList();
                
                var closingIdsToRemove = existingClosingIds.Except(request.Closings?.Where(c => c.Id.HasValue)?.Select(c => c.Id.Value) ?? new List<Guid>()).ToList();
                if (closingIdsToRemove.Any())
                {
                    product.RemoveClosings(closingIdsToRemove);
                    await _context.SaveChangesAsync(token);
                }

                var ids = new List<Guid>();
                if (request.Closings != null)
                {
                    foreach (var closing in request.Closings)
                    {
                        var result =
                            await _mediatr.Process(
                                new UpdateOrCreateProductClosingCommand(request.RequestUser)
                                    {ProductId = request.ProductId, Closing = closing}, token);

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