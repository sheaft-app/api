using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class DeleteUboCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class DeleteUboCommandHandler : CommandsHandler,
        IRequestHandler<DeleteUboCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public DeleteUboCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<DeleteUboCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(DeleteUboCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(
                    c => c.Declaration.Ubos.Any(u => u.Id == request.Id), token);
                legal.Declaration.RemoveUbo(request.Id);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
