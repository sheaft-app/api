using System;
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
using Sheaft.Domain;

namespace Sheaft.Application.Tag.Commands
{
    public class DeleteTagCommand : Command
    {
        [JsonConstructor]
        public DeleteTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteTagCommandHandler : CommandsHandler,
        IRequestHandler<DeleteTagCommand, Result>
    {
        public DeleteTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        
        public async Task<Result> Handle(DeleteTagCommand request, CancellationToken token)
        {
                var entity = await _context.GetByIdAsync<Domain.Tag>(request.Id, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Success();
        }
    }
}
