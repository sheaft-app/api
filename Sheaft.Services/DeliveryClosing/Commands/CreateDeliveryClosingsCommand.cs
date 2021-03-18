using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Services.DeliveryClosing.Commands
{
    public class CreateDeliveryClosingsCommand : Command<List<Guid>>
    {
        [JsonConstructor]
        public CreateDeliveryClosingsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public List<CreateClosingDto> Closings { get; set; }
    }

    public class CreateDeliveryClosingsCommandHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryClosingsCommand, Result<List<Guid>>>
    {
        public CreateDeliveryClosingsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateDeliveryClosingsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<List<Guid>>> Handle(CreateDeliveryClosingsCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var ids = new List<Guid>();
                foreach (var closing in request.Closings)
                {
                    var result = await _mediatr.Process(
                        new CreateDeliveryClosingCommand(request.RequestUser)
                            {Closing = closing, DeliveryId = request.DeliveryId}, token);
                    
                    if (!result.Succeeded)
                        return Failure<List<Guid>>(result.Exception);

                    ids.Add(result.Data);
                }

                await transaction.CommitAsync(token);
                return Success(ids);
            }
        }
    }
}