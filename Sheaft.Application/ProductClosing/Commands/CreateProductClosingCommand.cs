using System;
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
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.ProductClosing.Commands
{
    public class CreateProductClosingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateProductClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public ClosingInput Closing { get; set; }
    }

    public class CreateProductClosingCommandHandler : CommandsHandler,
        IRequestHandler<CreateProductClosingCommand, Result<Guid>>
    {
        public CreateProductClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateProductClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateProductClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.AddClosing(request.Closing.From, request.Closing.To, request.Closing.Reason);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}