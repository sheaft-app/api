using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Ubo.Commands
{
    public class DeleteUboCommand : Command
    {
        protected DeleteUboCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UboId { get; set; }
    }

    public class DeleteUboCommandHandler : CommandsHandler,
        IRequestHandler<DeleteUboCommand, Result>
    {
        public DeleteUboCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteUboCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteUboCommand request, CancellationToken token)
        {
            var legal = await _context.Set<BusinessLegal>()
                .SingleOrDefaultAsync(c => c.Declaration.Ubos.Any(u => u.Id == request.UboId), token);
            legal.Declaration.RemoveUbo(request.UboId);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}