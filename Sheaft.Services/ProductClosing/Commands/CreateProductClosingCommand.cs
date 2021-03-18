using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Services.ProductClosing.Commands
{
    public class CreateProductClosingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateProductClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public CreateClosingDto Closing { get; set; }
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