using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.ProductClosing.Commands
{
    public class UpdateOrCreateProductClosingCommand : Command<Guid>
    {
        [JsonConstructor]
        public UpdateOrCreateProductClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public UpdateOrCreateClosingDto Closing { get; set; }
    }

    public class UpdateOrCreateProductClosingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateProductClosingCommand, Result<Guid>>
    {
        public UpdateOrCreateProductClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateProductClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(UpdateOrCreateProductClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            Guid closingId;
            if (!request.Closing.Id.HasValue)
            {
                var closing = entity.Closings.SingleOrDefault(c => c.Id == request.Closing.Id);
                if (closing == null)
                    throw SheaftException.NotFound();

                closing.ChangeClosedDates(request.Closing.From, request.Closing.To);
                closing.SetReason(request.Closing.Reason);
                closingId = closing.Id;
            }
            else
            {
                var result = entity.AddClosing(request.Closing.From, request.Closing.To, request.Closing.Reason);
                closingId = result.Id;
            }

            await _context.SaveChangesAsync(token);
            return Success(closingId);
        }
    }
}