using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Tag.Commands
{
    public class DeleteTagCommand : Command
    {
        [JsonConstructor]
        public DeleteTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
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
                var entity = await _context.GetByIdAsync<Domain.Tag>(request.TagId, token);
                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
                return Success();
        }
    }
}
