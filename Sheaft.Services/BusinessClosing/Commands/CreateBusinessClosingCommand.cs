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

namespace Sheaft.Services.BusinessClosing.Commands
{
    public class CreateBusinessClosingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateBusinessClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public CreateClosingDto Closing { get; set; }
    }

    public class CreateBusinessClosingCommandHandler : CommandsHandler,
        IRequestHandler<CreateBusinessClosingCommand, Result<Guid>>
    {
        public CreateBusinessClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateBusinessClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateBusinessClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Business>(request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.AddClosing(request.Closing.From, request.Closing.To, request.Closing.Reason);
            await _context.SaveChangesAsync(token);
            
            return Success(entity.Id);
        }
    }
}