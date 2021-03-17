using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Ubo.Commands
{
    public class DeleteUboCommand : Command
    {
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
            var legal = await _context.GetSingleAsync<BusinessLegal>(
                c => c.Declaration.Ubos.Any(u => u.Id == request.UboId), token);
            legal.Declaration.RemoveUbo(request.UboId);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}