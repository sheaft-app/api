using System;
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
using Sheaft.Domain;

namespace Sheaft.Application.Ubo.Commands
{
    public class DeleteUboCommand : Command
    {
        [JsonConstructor]
        public DeleteUboCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class DeleteUboCommandHandler : CommandsHandler,
        IRequestHandler<DeleteUboCommand, Result>
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

        public async Task<Result> Handle(DeleteUboCommand request, CancellationToken token)
        {
            var legal = await _context.GetSingleAsync<BusinessLegal>(
                c => c.Declaration.Ubos.Any(u => u.Id == request.Id), token);
            legal.Declaration.RemoveUbo(request.Id);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}